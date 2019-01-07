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
         * Display a mesage box that gives guidance on the use and ordering of the parsing functions
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

            // Chooses player#.csv and parses into seperate path .csv files
            playerPathParser.SelectAndParsePlayerCSVIntoPaths();
        }

        /*
         * Combines seperate .csv files into one master.csv file
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
         * dictated by the nested for loops, and reliant upon the min max query data perfomed after creating the database.
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
                string outputFileName = "velocityColumn." + x + ".csv";
                // and open it. 
                StreamWriter outputStream = new StreamWriter(outputFileName);

                // Gets the averaged velocity data for the column x
                // The two dimensional array is a structure required if the query is to be peromed as a single function rather than a 
                // stepped approach. This non-stepped functionality is within the MasterTable.SegmentDatabaseIntoArray() function
                Vector[,] result = m_MasterTable.SegmentDatabaseIntoArray(Experiment.LevelAxisSubdivisions, x);

                // Intialise an empty string
                string line = "";

                // For each value in the query result, write a comma seperated value to the string
                for (int y = 0; y < Experiment.LevelAxisSubdivisions; y++)
                {
                    // ToMyString() returns a single NON-comma seperated value. Here we use the arbitary non numerical char '@'
                    // to allow an explicit string.Split('@') later that does not warp the comma seperated nature of .csv tables
                    // and maintains the correct columns per item
                    line += result[x, y].ToMyString() + ",";
                }

                // Write the line to the output
                outputStream.WriteLine(line);

                // Finally close the cuurent output
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
            DrawAndSaveDataVisualisation();
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
        }

        /*
         * Draw and save the data visualisation
         */
        private void DrawAndSaveDataVisualisation()
        {
            // Create the graphics objects needed for the displaying and saving of the data visualisation
            Bitmap returnBMP = new Bitmap(VectorMapPictureBox.Width, VectorMapPictureBox.Height);
            Graphics returnGraphics = Graphics.FromImage(returnBMP);

            //
            float xStep = (float)VectorMapPictureBox.Width / Experiment.LevelAxisSubdivisions;
            float yStep = (float)VectorMapPictureBox.Height / Experiment.LevelAxisSubdivisions;

            // The max size of a velocity arrow is the game rule's max allowed speed. An average cannot be higher
            float maxSize = InfernoLevelData.maxPlayerSpeed;

            // Create the pen
            Pen pen = new Pen(Color.Black, 1.0f)
            {
                // Make the line look nice with an arrow pointing the relevant direction
                //     StartCap = System.Drawing.Drawing2D.LineCap.RoundAnchor,
                EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor
            };
            
            // For each potential tile, look up in velocity trend in m_LedReckoningLevelDataTable and try to draw it
            for (int x = 0; x < Experiment.LevelAxisSubdivisions; x++)
            {
                for (int y = 0; y < Experiment.LevelAxisSubdivisions; y++)
                {
                    // Draw the map the right way around, reverse the 0-99 y value - Just aesthetic convention shows Map level De_Inferno being this way up
                    // Get the velocity trend value stored in the Vectpr[,] m_LedReckoningLevelDataTable for that tile
                    Vector velocityTrend = m_LedReckoningLevelDataTable[x, Experiment.LevelAxisSubdivisions - y - 1];

                    // Only draw a line if there is a non zero vector there
                    if (velocityTrend.Length != 0.0)
                    {
                        // Set the colour as a grayscale currently ish (/2)
                        pen.Color = Color.FromArgb(
                            128,
                            (int)velocityTrend.Length / 2,
                            (int)velocityTrend.Length / 2,
                            (int)velocityTrend.Length / 2);

                        // Start drawing from the centre of the tile
                        float startX = (x + 0.5f) * xStep;
                        float startY = (y + 0.5f) * yStep;

                        // * Step / (2.0f * maxSize) limits the draw to the edge of the tile
                        float endX = startX + (velocityTrend.X * xStep / (0.1f * maxSize));
                        float endY = startY + (velocityTrend.Y * yStep / (0.1f * maxSize));

                        // Draw it
                        returnGraphics.DrawLine(pen, startX, startY, endX, endY);
                    }
                }
            }

            // Dispose the pen
            pen.Dispose();

            // Save the new image as a .png
            returnBMP.Save("visualisation.png", System.Drawing.Imaging.ImageFormat.Png);

            // Set the image of the Winforms picture box to the new image
            VectorMapPictureBox.Image = returnBMP;
            
            // Success message
            MessageBox.Show("Image saved as visualisation.png.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            // Set next ui for experiment to active
            loadPlayerPathsToExperimentUponToolStripMenuItem.Enabled = true;
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
         *      -- incorporate threshold stuff. Seperate experiment?
         *      -- Like maybe a path long experiment that tracks a frame by frame displacement
         *      - incorporate std dead reckoning protocol like update after 5 secs etc
         *      
         * - Make the damn winforms look better
         * 
         */
    }
}
