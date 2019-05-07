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
    /*
     * Read me!
     * 
     * The methods used to conduct this experient have varied muchly over the course of this artefact's development.
     * 
     * Now, all functionality is automated by the 'Experiment'->'Auto sampling experiment' button in the WinForms application
     * This calls the AutoSamplingExperimentToolStripMenuItem_Button_Click() function below.
     * 
     * Also, testing is performed with the 'Test'->'Test against control data' button which calls the testAgainstControlDataToolStripMenuItem_Click()
     * function.
     * 
     * What remains far lower in this program serves as a record of the different approaches I have tried (and have got working - although
     * to different degrees of performance). Although through iteration, methods like the database driven approach went unused in
     * collecting the final data, they did form part of the process of this program getting to this point so are included in the interests
     * of completeness. Rightly ignore if you like, but I didn't want to delete them from this as they were part of the journey
     * and might hold some use yet still... e.g. the minimum and maximum position values were derived using these methods.
     */
    public partial class mainForm : Form
    {
        // Will perform this many experiments using the 'Auto Sampling Method' used for the final results.
        // Useful for collecting the 500 results that I used in the dissertation
        int numberOfExperimentsToPerform = 1;

        // Object references to data shared between different functions
        // A pathloading class that loads a CSV path and stores it in memory for quick access
        PlayerPathLoader m_PlayerPathLoader;

        // The trend map data structure that led reckoning relies upon
        Vector[,] m_LedReckoningLevelDataTable;

        // A singleton of the class responsible for drawing results to the WinForms PictureBox GUI
        Visualiser m_Visualiser;

        // For testing purposes.
        // Allows using the same experimenting function as normal but for different outcomes
        bool isTesting = false;

        /*
         * The Constructor for the Winforms mainForm
         */
        public mainForm()
        {
            // Winforms initialisation
            InitializeComponent();

            // Initialise the Vector[,] data structure for the LedReckoning data
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
         * The main experiment. Automated to choose random experiment subjects from the data set,
         * process the trend map, and perform an experiment, outputting the results
         */
        private void AutoSamplingExperimentToolStripMenuItem_Button_Click(object sender, EventArgs e)
        {
            // Choose all path.csv files for the experiment
            // Displays an OpenFileDialog so the user can select the .csv files.  
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files|*.csv";
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Select all path.csv files";

            // Show the Dialog. If the user clicked OK in the dialog and a .csv file was selected, open it.  
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // All of the filenames that have been selected
                string[] fileNames = openFileDialog.FileNames;

                // The filename of the experiment subject
                string experimentSubject;

                // If performing multiple experiments with the data set, this will loop the experiment for the numberOfExperimentsToPerform amount
                for (int experimentNumber = 0; experimentNumber < numberOfExperimentsToPerform; experimentNumber++)
                {
                    //---------------------------------
                    // SELECTION
                    //---------------------------------

                    // If testing then force the experiment subject to be fileNames[0]
                    if (isTesting)
                    {
                        // For testing
                        experimentSubject = fileNames[0];
                    }
                    else
                    {
                        // Else randomly select an experiment subject from the filenames
                        experimentSubject = fileNames[new Random().Next(fileNames.Length)];
                    }

                    // Singleton reference for experiment
                    m_PlayerPathLoader = new PlayerPathLoader();

                    // Loads in the path and performs a zero length path check
                    if (!m_PlayerPathLoader.LoadCSVPath(experimentSubject))
                    {
                        // If zero length then inform user
                        MessageBox.Show("Selected path travels zero distance. Perform experiment again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                        // If not testing then stop here. There is no reason to include zero path experiment predictions in the dissertation results
                        // However, if testing, then may be testing for zero velocity path prediction. Allow to continue to complete test.
                        if (!isTesting) return;
                    }
                    
                    //---------------------------------
                    // VELOCITY TRENDS
                    //---------------------------------

                    // Declare the data structure to accumulate velocities before averaging them
                    AreaVelocityInfo[,] cumalativeVelocityTable = new AreaVelocityInfo[Experiment.LevelAxisSubdivisions, Experiment.LevelAxisSubdivisions];

                    // Initialise the data structure to zero values
                    for (int x = 0; x < Experiment.LevelAxisSubdivisions; x++)
                    {
                        for (int y = 0; y < Experiment.LevelAxisSubdivisions; y++)
                        {
                            cumalativeVelocityTable[x, y].accumulativeVelocity = new Vector(0.0f);
                            cumalativeVelocityTable[x, y].count = 0;
                        }
                    }

                    // Used for reading the CSV files
                    string currentLine;
                    string[] lineItems;

                    // Reads through all of the selected path files and accumulates them
                    foreach (string fileName in fileNames)
                    {
                        // Don't include the experimented upon file
                        if (fileName != experimentSubject)
                        {
                            using (StreamReader stringReader = new StreamReader(fileName))
                            {
                                // First line is the column headers - ignore
                                currentLine = stringReader.ReadLine();

                                // Sanity check that the next line is not null
                                if ((currentLine = stringReader.ReadLine()) != null)
                                {
                                    // Get the comma separated line items
                                    lineItems = currentLine.Split(',');

                                    // Get the team from the experiment subject
                                    // Check if the team name of the currently looked at path matchs the experimentSubject team name
                                    // We only want to build a trend map of the specific team that the subject belongs to. See dissertation
                                    if (lineItems[1] == m_PlayerPathLoader.teamName)
                                    {
                                        do
                                        {
                                            // Get the comma separated line items
                                            lineItems = currentLine.Split(',');

                                            // positionX, positionY from the csv structure as outlined in CSVWriter.cs
                                            int[] lookUpCoords = InfernoLevelData.TranslatePositionIntoLookUpCoordinates(double.Parse(lineItems[2]), double.Parse(lineItems[3]));

                                            // As a precaution against partially formed CSV lines in the path as parsed from the DEM file
                                            try
                                            {
                                                // velocityX, velocityY, and velocityZ from the csv structure as outlined in CSVWriter.cs
                                                cumalativeVelocityTable[lookUpCoords[0], lookUpCoords[1]].accumulativeVelocity += new Vector(lineItems[8], lineItems[9], lineItems[10]);
                                                cumalativeVelocityTable[lookUpCoords[0], lookUpCoords[1]].count++;
                                            } catch{}

                                            // CurrentLine will be null when the StreamReader reaches the end of file
                                        } while ((currentLine = stringReader.ReadLine()) != null);
                                    }
                                }
                            }
                        }
                    }

                    // Averages the values and stores them in the Vector trend data structure
                    // Use the count from the AreaVelocityInfo to calculate the average.
                    // Look at each entry in the cumalativeVelocityTable
                    for (int x = 0; x < Experiment.LevelAxisSubdivisions; x++)
                    {
                        for (int y = 0; y < Experiment.LevelAxisSubdivisions; y++)
                        {
                            // Check for a zero count to avoid divide by zero errors
                            if (cumalativeVelocityTable[x, y].count != 0)
                            {
                                // Set the Velocity trend value to the mean average value calculated from the cumalativeVelocityTable
                                m_LedReckoningLevelDataTable[x, y] = cumalativeVelocityTable[x, y].accumulativeVelocity / cumalativeVelocityTable[x, y].count;
                            }
                            else
                            {
                                // If count is zero then set to zero value
                                m_LedReckoningLevelDataTable[x, y] = new Vector(0.0f);
                            }
                        }
                    }

                    //---------------------------------
                    // EXPERIMENT
                    //---------------------------------

                    // Do experiment with the chosen path.
                    // Starts a new experiment with the appropriate previously chosen data
                    Experiment experiment = new Experiment(m_PlayerPathLoader, m_LedReckoningLevelDataTable);

                    // Diplays a visual representation into the winforms picture box
                    m_Visualiser = new Visualiser(VectorMapPictureBox, m_LedReckoningLevelDataTable);

                    // Shows and saves visualisation as a .png image in the program's directory
                    m_Visualiser.DrawAndSavePathVisualisation(experiment);

                    //---------------------------------
                    // TESTING
                    //---------------------------------

                    // For testing. Construct an expected results string and compare it to a passed in file expecting the correct results
                    if (isTesting)
                    {
                        // The expected results string
                        string expResults = $"{m_PlayerPathLoader.pathName}," +
                            $"{m_PlayerPathLoader.pathDictionary.Count}," +
                            $"{Math.Round(experiment.totalDeadReckoningDistanceFromActual / m_PlayerPathLoader.pathDictionary.Count, 8)}," +
                            $"{experiment.m_TraditionalDeadReckoning.numberOfUpdatesNeeded}," +
                            $"{Math.Round(experiment.totalLedReckoningDistanceFromActual / m_PlayerPathLoader.pathDictionary.Count, 8)}," +
                            $"{experiment.m_LedReckoning.numberOfUpdatesNeeded}";

                        // User instruction
                        MessageBox.Show("Select APPROPRIATE CORRECT results to test against", "Instructions", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                        // Displays an OpenFileDialog so the user can select a .csv file.  
                        openFileDialog = new OpenFileDialog();
                        openFileDialog.Filter = "CSV files|*.csv";
                        openFileDialog.Multiselect = false;
                        openFileDialog.Title = "Select CorrectResults.CSV file";

                        // Show the Dialog. If the user clicked OK in the dialog and a .csv file was selected, open it.  
                        if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            // Open up an IDisposable StreamReader for the file
                            using (StreamReader streamReader = new StreamReader(openFileDialog.FileName))
                            {
                                // First line is the column headers - ignore
                                streamReader.ReadLine();

                                // The next line is the predetermined results data
                                string controlResults = streamReader.ReadLine();

                                // The next line in results data holds predertimined information upon the path
                                string[] positionCheckArray = streamReader.ReadLine().Split(',');

                                // These bools check the results in the file against the results from the experiment
                                bool Actual_StartingPositionsMatch = positionCheckArray[0] == experiment.initialPosition.ToMyEasierSplitString();

                                bool Actual_StartingVelocitiesMatch = positionCheckArray[1] == experiment.initialVelocity.ToMyEasierSplitString();
                                
                                bool Actual_FinalPositionsMatch = positionCheckArray[2] == experiment.currentPosition.ToMyEasierSplitString();
                                bool DR_FinalPositionsMatch = positionCheckArray[2] == experiment.m_TraditionalDeadReckoning.deadReckonedPosition.ToMyEasierSplitString();
                                bool LR_FinalPositionsMatch = positionCheckArray[2] == experiment.m_LedReckoning.deadReckonedPosition.ToMyEasierSplitString();

                                bool Actual_FinalVelocitiesMatch = positionCheckArray[3] == experiment.currentVelocity.ToMyEasierSplitString();
                                bool DR_FinalVelocitiesMatch = positionCheckArray[3] == experiment.m_TraditionalDeadReckoning.simulatedVelocity.ToMyEasierSplitString();
                                bool LR_FinalVelocitiesMatch = positionCheckArray[3] == experiment.m_LedReckoning.simulatedVelocity.ToMyEasierSplitString();

                                // Show comparison results message as multiple lines of 'True' or 'False' for tests passing or failing
                                // Multiple comparisons are sometimes made for a test to pass
                                MessageBox.Show($"{experiment.experimentName}\n" +
                                    $"Test results match: {controlResults == expResults}\n" +
                                    $"Starting positions match: {Actual_StartingPositionsMatch}\n" +
                                    $"Starting velocities match: {Actual_StartingVelocitiesMatch}\n" +
                                    $"Ending positions match: {Actual_FinalPositionsMatch && DR_FinalPositionsMatch && LR_FinalPositionsMatch}\n" +
                                    $"Ending velocities match: {Actual_FinalVelocitiesMatch && DR_FinalVelocitiesMatch && LR_FinalVelocitiesMatch}", "Test", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                            }
                        }
                    }
                }
            }
        }

        /*
         * Combines multiple selected experiment results files into one file for a data analysis program to analyse
         * 
         * Used after 500 experiments were completed for the dissertation, and I wanted to combine them into one CSV
         * file to load into a data analysis program.
         */
        private void combineResultsCSVFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Displays an OpenFileDialog so the user can select a .csv file.  
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files|*.csv";
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Select all experiment.CSV files";

            // Show the Dialog. If the user clicked OK in the dialog and .csv files were selected, open it.  
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // The selected file names
                string[] fileNames = openFileDialog.FileNames;

                // To ensure no results are duplicated, given that they are randomly chosen each time,
                // we keep track of all experiment result filenames added
                List<string> namesAdded = new List<string>();

                // For reading the CSV file
                string currentLine;

                // Create the output file stream
                StreamWriter outputStream = new StreamWriter("results.CSV");

                // Write the column headers.
                outputStream.WriteLine("PathName," +
                    "NumFrames," +
                    "Average dead reckoned error distance," +
                    "DeadUpdates," +
                    "Average led reckoned error distance," +
                    "LedUpdates");

                // For each file selected
                foreach (string fileName in fileNames)
                {
                    // Check if name has already been added to avoid duplicates
                    if (!namesAdded.Contains(fileName))
                    {
                        // If not already included then include and add to the list for future checks
                        namesAdded.Add(fileName);

                        // Use 'using' keyword for memory clean up when finished with the StreamReader
                        using (StreamReader stringReader = new StreamReader(fileName))
                        {
                            // First line is the column headers - ignore
                            currentLine = stringReader.ReadLine();

                            // Second line is the information that we want to write to the output stream
                            outputStream.WriteLine(stringReader.ReadLine());
                        }
                    }
                }

                // Close the output stream
                outputStream.Close();

                // Show success message
                MessageBox.Show("Finished combining results files.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        /*
         * Testing is performed by setting the isTesting bool, then executing a typical experiment
         */
        private void testAgainstControlDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Set the bool so that the experiment writes and compares the results data in a test environment
            isTesting = true;

            // Perform the experiment as normal except with the isTesting bool set
            AutoSamplingExperimentToolStripMenuItem_Button_Click(sender, e);

            // Reset the bool for future experiments
            isTesting = false;
        }

        /*
         * Zooms in on the GUI image by resizing the PictureBox
         */
        private void ZoomScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            int zoomStrength = 50;
            int scrollValue = e.NewValue - e.OldValue;

            // Add the width and height to the picture box dimensions
            VectorMapPictureBox.Width += scrollValue * zoomStrength;
            VectorMapPictureBox.Height += scrollValue * zoomStrength;
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

        //----------------------------------------------------------//
        //----------------------------------------------------------//
        // Here lie the previous methods as detailed in the read me //
        //----------------------------------------------------------//
        //----------------------------------------------------------//

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

        /*
         * Combines separate .csv files into one master.csv file
         * Used to collate ALL of the data into one file
         */
        private void CombineMultipleCSVFilesIntoMasterCSV(object sender, EventArgs e)
        {
            // Static function handles choosing the files and combining them
            CombineCSVFiles.SelectAndCombineMultipleCSVFilesIntoMasterCSV();
        }

        /*
         * Allows the user to select, open, and reference an existing database.db file
         */
        private void ConnectToExistingDatabase(object sender, EventArgs e)
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
            }
        }

        /*
         * Creates a database table from the completed, cumulated master.csv file
         */
        private void CreateNewDatabaseFromMastercsvFile(object sender, EventArgs e)
        {
            // Create the new database 
            m_Database = new SQLDatabase(databaseName);

            // Set up the table
            m_MasterTable = m_Database.addMasterTable(masterTableName, masterTableColumns);

            // Displays an OpenFileDialog so the user can select a .csv file.  
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV file|*.csv";
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
                }
            }
        }

        /*
         * DEPRECATED METHOD 
         * But it looks like I spent a long time writing this when I was doing the database approach so have left in as 
         * a hat tip to the previous method.
         * 
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
         */
        private void CreateAverageVelocityTrendMasterCSVFromDatabase(object sender, EventArgs e)
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
        }

        /*
         * If using a stepped database parsing method, use this function to combine each row of aggregated and averaged tiles into one whole
         */
        private void CombineVelocityColumnCSVFilesIfQueryingSeperately(object sender, EventArgs e)
        {
            // Static function handles choosing the files and combining according to the # in velocityColumn.#.csv
            m_LedReckoningLevelDataTable = CSVParser.ConstructMasterVelocityCSVFromVelocityColumnCSVs();
        }

        /*
         * Allows the user to choose and load a led reckoning velocity trend .csv into memory as a Vector[,]
         */
        private void LoadLedReckoningMasterVelocityCSV(object sender, EventArgs e)
        {
            // Static function allows the user to select a previously constructed velocity trend .csv file and load it into memory as a Vector[,]
            m_LedReckoningLevelDataTable = MasterArray.LoadLedReckoningData();

            // Diplays a visual representation into the winforms picture box, and saves the graphics as a .png image in the program's directory
            m_Visualiser = new Visualiser(VectorMapPictureBox, m_LedReckoningLevelDataTable);
            m_Visualiser.DrawAndSaveDataVisualisation();
        }

        /*
         * Performs minimum and maximum queries upon the database table and shows the result in a message box
         * Used to subsequently construct further queries when segmenting the data (see InfernoLevel/InfernoLevelData.cs)
         */
        private void QueryMinMaxPositionValues(object sender, EventArgs e)
        {
            // Executes the minimum and maximum queries required and displays the results in a message box
            m_MasterTable.GetMaxMin();
        }

        /*
         * Allows the user to select any number of player paths for a random sample to be chosen from and loaded into memory
         */
        private void LoadPlayerPathsToExperimentUpon(object sender, EventArgs e)
        {
            // Singleton reference required for experiment function to see the data
            m_PlayerPathLoader = new PlayerPathLoader();
            
            // A path is deemed valid if it is of non zero length. There is no point in wasting time experimenting on these
            if (m_PlayerPathLoader.ChooseValidPath())
            {
                // Set next UI for experiment to active
            //    PerformExperiment_ToolStripMenuItem.Enabled = true;
            }
        }

        /*
         * Uses the m_LedReckoningLevelDataTable, and the player path, both chosen previously to this function being made available
         * to perform a frame by frame experiment. The player path simultaneously has a dead reckoning and a led reckoning approach
         * applied, and the accuracy measured as an average distance of the simulated position from the actual position, and the total
         * number of threshold driven simulated packet updates required, are stored in the resulting .csv file
         */
        private void PerformExperiment(object sender, EventArgs e)
        {
            // Starts a new experiment with the appropriate previously chosen data
            Experiment experiment = new Experiment(m_PlayerPathLoader, m_LedReckoningLevelDataTable);

            // Show and save visualisation
            m_Visualiser.DrawAndSavePathVisualisation(experiment);
        }

        /*
         * Proceses the large Master.CSV file into the data structure
         */
        private void loadMasterCSVIntoMemory(object sender, EventArgs e)
        {
            // Constructor of instance prompts selecting and processing of path data
            MasterArray masterArray = new MasterArray();

            // Set the reference
            m_LedReckoningLevelDataTable = masterArray.averageVelocityTable;

            // Draw and save the resulting data
            m_Visualiser = new Visualiser(VectorMapPictureBox, m_LedReckoningLevelDataTable);
            m_Visualiser.DrawAndSaveDataVisualisation();
        }
    }
}
