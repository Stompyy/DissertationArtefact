using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace csgoDemoParser
{
    public partial class mainForm : Form
    {
        // Constant string declarations of data structure descriptors
        const string databaseName = "database.db";
        const string masterTableName = "master";
        const string masterTableColumns =
            "name varchar(128) NOT NULL, " +
            "team varchar(128) NOT NULL, " +
            "positionX double NOT NULL, " +
            "positionY double NOT NULL, " +
            "positionZ double NOT NULL, " +
            "activeWeapon varchar(24) NOT NULL, " +
            "hp double NOT NULL, " +
            "armour double NOT NULL, " +
            "velocityX double NOT NULL, " +
            "velocityY double NOT NULL, " +
            "velocityZ double NOT NULL, " +
            "isDucking int NOT NULL, " +
            "hasHelmet int NOT NULL, " +
            "hasDefuseKit int NOT NULL, " +
            "viewDirectionX double NOT NULL, " +
            "viewDirectionY double NOT NULL ";

        // Object references to data shared between different functions
        SQLDatabase m_Database;
        MasterTable m_MasterTable;
        PlayerPathLoader m_PlayerPathLoader;
        Vector[,] m_LedReckoningLevelDataTable;
        Visualiser m_Visualiser;

        /*
         * The Constructor for the Winforms mainForm
         */
        public mainForm()
        {
            // Winforms initialisation
            InitializeComponent();

            // Initialise the Vector[,] for the LedReckoning data
            m_LedReckoningLevelDataTable = new Vector[Experiment.LevelAxisSubdivisions, Experiment.LevelAxisSubdivisions];
        }

        /*
         * Display a message box that gives guidance on the use and ordering of the parsing functions
         */
        private void HelpButton_Click(object sender, EventArgs e)
        {
            // Show the message box
            MessageBox.Show(HelpMessage.getMessage(), "Help", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        /*
         * Handles the choosing and parsing of a .dem csgo replay file into a .csv file
         */
        private void ParseDemFileToCSV_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Static function handles choosing the file and parsing it
            RawDataParser.SelectAndParseDemFile();
        }

        /*
         * Takes a parsed game .csv file and writes a new .csv file for each player in that game 
         */
        private void SeperateRawDataCSVIntoPlayers_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Static function handles choosing the file and parsing it
            CSVParser.SelectAndParseRawDataCSV();
        }

        /*
         * Takes a parsed player file and uses a distance check to write a new .csv file for each path the player has taken
         * The distance check accounts for dying then respawning, or breaks between rounds
         */
        private void SeperatePlayercsvIntoPaths_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Non static function as data is shared and accumulated by the program. Instance makes design sense
            PlayerPathParser playerPathParser = new PlayerPathParser();

            // Chooses player#.csv and parses into separate path .csv files
            playerPathParser.SelectAndParsePlayerCSVIntoPaths();
        }

        /*
         * Combines separate .csv files into one master.csv file
         * Used to collate ALL of the data into one file
         */
        private void CombineMultipleCSVFilesIntoMasterCSV_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Static function handles choosing the files and combining them
            CombineCSVFiles.SelectAndCombineMultipleCSVFilesIntoMasterCSV();
        }

        /*
         * Allows the user to select, open, and reference an existing database.db file
         */
        private void ConnectToExistingDatabase_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Displays an OpenFileDialog so the user can select a .db file.  
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "database files|*.db";
            openFileDialog.Title = "Select database.db file";

            // Show the Dialog. If the user clicked OK in the dialog and a .db file was selected, open it.  
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Connect to the database, open the connection, and set the reference to the table
                m_MasterTable = new MasterTable(openFileDialog.FileName, masterTableName);

                // Enable the next UI
                QueryMinMaxPositionValues_ToolStripMenuItem.Enabled = true;
                CreateAverageVelocityTrendMasterCSVFromDatabase_ToolStripMenuItem.Enabled = true;
            }
        }

        /*
         * Creates a database table from the completed, cumulated master.csv file
         */
        private void CreateNewDatabaseFromMastercsvFile_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create the new database 
            m_Database = new SQLDatabase(databaseName);

            // Set up the table
            m_MasterTable = m_Database.addMasterTable(masterTableName, masterTableColumns);

            // Displays an OpenFileDialog so the user can select a .csv file.  
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files|*.csv";
            openFileDialog.Title = "Select master.csv file";

            // Show the Dialog. If the user clicked OK in the dialog and a .csv file was selected, open it.  
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (StreamReader stringReader = new StreamReader(openFileDialog.FileName))
                {
                    // First line is the column headers - ignore
                    string currentLine = stringReader.ReadLine();

                    // Bulk insert creates an insert command that will contain every insert needed before executing
                    // Ridiculously faster than doing inserts one by one, or even 100 by 100
                    m_MasterTable.BulkInsert(stringReader);
                    
                    // Enable the next UI
                    QueryMinMaxPositionValues_ToolStripMenuItem.Enabled = true;
                    CreateAverageVelocityTrendMasterCSVFromDatabase_ToolStripMenuItem.Enabled = true;
                }
            }
        }

        /*
         * This is the big one.
         * Performs multiple queries upon the database for positions within bounds
         * Returns the velocities for those entries and averages them before writing to .csv file
         * The aim is to get a data structure Experiment.LevelAxisSubdivisions * Experiment.LevelAxisSubdivisions large
         * where each entry represents the average velocity of all database entries where the position falls within the bounds
         * dictated by the nested for loops, and reliant upon the min max query data performed after creating the database.
         * 
         * For a stepped approach that allowed me to run this program over multiple occasions, we choose to write a single line 
         * of queries at a time, saving a new csv file (named according to its row number) each time. Depending on the size
         * of the query, each row of Experiment.LevelAxisSubdivisions number queries takes between 17 - 55 minutes to complete and 
         * write (for an ~ 8GB database, and depending on which computer I was using. This approach also allowed me to use simultaneous 
         * programs on different machines. The multiple resulting csv files are then combined in the next function CombineVelocityColumnCSVFilesIfQueryingSeperately_ToolStripMenuItem_Click()
         * 
         * Ultimately this speed inefficiency is apparently a misuse of SQLite.
         * A different database approach would have yielded a much faster query rate. I chose SQLite due to having had some experience with it
         * and once realising the inefficiency, the work was nearly complete.
         */
        private void CreateAverageVelocityTrendMasterCSVFromDatabase_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // The grid x coordinates for the returned .csv file
            for (int x = 0; x < Experiment.LevelAxisSubdivisions; x++)
            {
                // For the stepped approach, name accordingly, and create the new file each x step
                // Create the fileName
                string outputFileName = $"velocityColumn.{x}.csv";
                // and open it. 
                StreamWriter outputStream = new StreamWriter(outputFileName);

                // Gets the averaged velocity data for the column x
                // The two dimensional array is a structure required if the query is to be performed as a single function rather than a 
                // stepped approach. This non-stepped functionality is within the MasterTable.SegmentDatabaseIntoArray() function
                Vector[,] result = m_MasterTable.SegmentDatabaseIntoArray(Experiment.LevelAxisSubdivisions, x);

                // Initialise an empty string
                string line = "";

                // For each value in the query result, write a comma separated value to the string
                for (int y = 0; y < Experiment.LevelAxisSubdivisions; y++)
                {
                    // ToMyString() returns a single NON-comma seperated value. Here we use the arbitary non numerical char '@'
                    // to allow an explicit string.Split('@') later that does not warp the comma separated nature of .csv tables
                    // and maintains the correct columns per item
                    line += result[x, y].ToMyString() + ",";
                }

                // Write the line to the output
                outputStream.WriteLine(line);

                // Finally close the current output
                outputStream.Close();
            }

            // When all complete, show success message
            MessageBox.Show("Finished velocity csv.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            // Set next UI to active
            CombineVelocityColumncsvFilesIfQueryingSeperatelyMain_ToolStripMenuItem.Enabled = true;
        }

        /*
         * If using a stepped database parsing method, use this function to combine each row of aggregated and averaged tiles into one whole
         */
        private void CombineVelocityColumnCSVFilesIfQueryingSeperately_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Static function handles choosing the files and combining according to the # in velocityColumn.#.csv
            m_LedReckoningLevelDataTable = CSVParser.ConstructMasterVelocityCSVFromVelocityColumnCSVs();
        }

        /*
         * Allows the user to choose and load a led reckoning velocity trend .csv into memory as a Vector[,]
         */
        private void LoadLedReckoningMasterVelocityCSV_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Static function allows the user to select a previously constructed velocity trend .csv file and load it into memory as a Vector[,]
            m_LedReckoningLevelDataTable = LedReckoning.LoadLedReckoningData();

            // Diplays a visual representation into the winforms picture box, and saves the graphics as a .png image in the program's directory
            m_Visualiser = new Visualiser(VectorMapPictureBox, m_LedReckoningLevelDataTable);
            m_Visualiser.DrawAndSaveDataVisualisation();

            // Set next ui for experiment to active
            loadPlayerPathsToExperimentUponToolStripMenuItem.Enabled = true;
        }

        /*
         * Debug tool - Following a long winded initial data collection period, 3/164 .dem files were discovered to be misnamed
         * as de_Inferno map, and actually contained other map data. This function is required to find the map names from the .dem files
         */
        private void CheckMapNamesOfdemSamples_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Static function allows the user to select multiple .dem files and writes a .csv file
            // with the replay names, and the map names taken from the game data
            Debug.WriteAllMapNamesToCSVFile();
        }

        /*
         * Performs minimum and maximum queries upon the database table and shows the result in a message box
         * Used to subsequently construct further queries when segmenting the data (see InfernoLevel/InfernoLevelData.cs)
         */
        private void QueryMinMaxPositionValues_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Executes the minimum and maximum queries required and displays the results in a message box
            m_MasterTable.GetMaxMin();
        }

        /*
         * Allows the user to select any number of player paths for a random sample to be chosen from and loaded into memory
         */
        private void LoadPlayerPathsToExperimentUpon_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Singleton reference required for experiment function to see the data
            m_PlayerPathLoader = new PlayerPathLoader();
            
            // A path is deemed valid if it is of non zero length. There is no point in wasting time experimenting on these
            if (m_PlayerPathLoader.ChooseValidPath())
            {
                // Set next UI for experiment to active
                PerformExperiment_ToolStripMenuItem.Enabled = true;
            }
        }

        /*
         * Uses the m_LedReckoningLevelDataTable, and the player path, both chosen previously to this function being made available
         * to perform a frame by frame experiment. The player path simultaneously has a dead reckoning and a led reckoning approach
         * applied, and the accuracy measured as an average distance of the simulated position from the actual position, and the total
         * number of threshold driven simulated packet updates required, are stored in the resulting .csv file
         */
        private void PerformExperiment_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Starts a new experiment with the appropriate previously chosen data
            Experiment experiment = new Experiment(m_PlayerPathLoader, m_LedReckoningLevelDataTable);

            // Show and save visualisation
            m_Visualiser.DrawAndSavePathVisualisation(experiment);
        }

        /*
         * 
         */
        private void loadMasterCSVIntoMemory_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MasterArray masterArray = new MasterArray();

            m_LedReckoningLevelDataTable = masterArray.averageVelocityTable;

            m_Visualiser = new Visualiser(VectorMapPictureBox, m_LedReckoningLevelDataTable);
            m_Visualiser.DrawAndSaveDataVisualisation();

            // Set next ui for experiment to active
            loadPlayerPathsToExperimentUponToolStripMenuItem.Enabled = true;
        }

        private void ZoomScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            int zoomStrength = 50;
            int scrollValue = e.NewValue - e.OldValue;

            //Add the width and height to the picture box dimensions

            VectorMapPictureBox.Width += scrollValue * zoomStrength;
            VectorMapPictureBox.Height += scrollValue * zoomStrength;
        }

        private void AutoSamplingExperimentToolStripMenuItem_Button_Click(object sender, EventArgs e)
        {
            // choose all path.csv files

            // Displays an OpenFileDialog so the user can select a .csv file.  
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files|*.csv";
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Select all path.csv files";

            // Show the Dialog. If the user clicked OK in the dialog and a .csv file was selected, open it.  
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // randomly choose one? many? How choose? rand int?
                // Time seeded
                string[] fileNames = openFileDialog.FileNames;
                string experimentSubject = fileNames[new Random().Next(fileNames.Length)];

                    Timer timer = new Timer();
                    timer.Start();

                // Keep to this local scope so that garbage collector can dispose of it when out of scope (hopefully)
                AreaVelocityInfo[,] cumalativeVelocityTable = new AreaVelocityInfo
                    [Experiment.LevelAxisSubdivisions, Experiment.LevelAxisSubdivisions];
                // Lengthy init of data structure. Can we not just set default values for our own objects that are not null?
                for (int x = 0; x < Experiment.LevelAxisSubdivisions; x++)
                {
                    for (int y = 0; y < Experiment.LevelAxisSubdivisions; y++)
                    {
                        cumalativeVelocityTable[x, y].cumulativeVelocity = new Vector(0.0f);

                        // I think as a numerical value, this already has a default value of 0
                        cumalativeVelocityTable[x, y].count = 0;
                    }
                }


                string currentLine;
                string[] lineItems;
                int acceptedLines = 0;
                int rejectedLines = 0;
                foreach (string fileName in fileNames)
                {
                    // Don't include the experimented upon file
                    if (fileName != experimentSubject)
                    {
                        using (StreamReader stringReader = new StreamReader(fileName))
                        {
                            // First line is the column headers - ignore
                            currentLine = stringReader.ReadLine();

                            // Read into trend thing
                            // CurrentLine will be null when the StreamReader reaches the end of file
                            while ((currentLine = stringReader.ReadLine()) != null)
                            {
                                // Find the playerName from the currentLine
                                lineItems = currentLine.Split(',');

                                // positionX, positionY from the csv structure as outlined in CSVWriter.cs
                                int[] lookUpCoords = InfernoLevelData.TranslatePositionIntoLookUpCoordinates(double.Parse(lineItems[2]), double.Parse(lineItems[3]));

                                // Sometimes incomplete lines?
                                try
                                {
                                    // velocityX, velocityY, and velocityZ from the csv structure as outlined in CSVWriter.cs
                                    cumalativeVelocityTable[lookUpCoords[0], lookUpCoords[1]].cumulativeVelocity += new Vector(lineItems[8], lineItems[9], lineItems[10]);
                                    cumalativeVelocityTable[lookUpCoords[0], lookUpCoords[1]].count++;
                                    acceptedLines++;
                                }
                                catch
                                {
                                    rejectedLines++;
                                }
                            }
                        }
                    }
                }

                    timer.Stop();

                // Success message
                MessageBox.Show("Finished cumulating non-experimented-upon .csv files. " +
                              $"{timer.GetTimeElapsed()} MS. " +
                    $"Accepted: {acceptedLines}. " +
                    $"Rejected: {rejectedLines}. " +
                    $"Experimented path: {experimentSubject}.",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                // This all feels like there should be less 'new' keywords being used...
                // Need to ensure that Experiment can properly pick up the averageVelocityTable and use it in the same way as before when deriving it from the database.


                // Use the count to calculate the average
                for (int x = 0; x < Experiment.LevelAxisSubdivisions; x++)
                {
                    for (int y = 0; y < Experiment.LevelAxisSubdivisions; y++)
                    {
                        if (cumalativeVelocityTable[x, y].count != 0)
                        {
                            m_LedReckoningLevelDataTable[x, y] = cumalativeVelocityTable[x, y].cumulativeVelocity / cumalativeVelocityTable[x, y].count;
                        }
                        else
                        {
                            m_LedReckoningLevelDataTable[x, y] = new Vector(0.0f);
                        }
                    }
                }

                // Do experiment with the chosen path.

                // Singleton reference required for experiment function to see the data
                m_PlayerPathLoader = new PlayerPathLoader();

                // Zero length path check
                if(!m_PlayerPathLoader.LoadCSVPath(experimentSubject))
                {
                    // Fail message
                    MessageBox.Show("Non-zero length path randomly selected. Perform experiment again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }

                // Starts a new experiment with the appropriate previously chosen data
                Experiment experiment = new Experiment(m_PlayerPathLoader, m_LedReckoningLevelDataTable);

                // Diplays a visual representation into the winforms picture box, and saves the graphics as a .png image in the program's directory
                m_Visualiser = new Visualiser(VectorMapPictureBox, m_LedReckoningLevelDataTable);

                // Show and save visualisation
                m_Visualiser.DrawAndSavePathVisualisation(experiment);
            }

        }



        /*
         * Still need to:
         * 
         * -- write a function that takes in the csv file on start and stores in an array (? or better data structure)
         * -- Write a function that takes in the actual position, and returns the 0-Experiment.LevelAxisSubdivisions (x, y) coords for look up
         * -- Then in LedReckoning.cs look up the trend
         * 
         * - Do stuff with acceleration
         * 
         * - Write the experiment:
         *      -- Need to update
         *      - Choose random path files? Random start and end points
         *      -- Shouldn't be a zero distance path. No point
         *      -- Output comparison results
         *      
         *      -- incorporate threshold stuff. Separate experiment?
         *      -- Like maybe a path long experiment that tracks a frame by frame displacement
         *      - incorporate std dead reckoning protocol like update after 5 secs etc
         *      
         * - Make the damn winforms look better
         * 
         */
    }
}
