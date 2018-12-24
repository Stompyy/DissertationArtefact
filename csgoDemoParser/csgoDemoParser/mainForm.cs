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
        const string databaseName = "database.db";
        const string masterTableName = "master";

        SQLDatabase m_Database;
        MasterTable m_MasterTable;

        Vector[,] ledReckoningLevelDataTable;


        /*
         * 
         */
        public mainForm()
        {
            InitializeComponent();

            ledReckoningLevelDataTable = new Vector[Experiment.LevelAxisSubdivisions, Experiment.LevelAxisSubdivisions];

            /*
            */
        }

        /*
         * 
         */
        private void ParseDemFileToCSV_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RawDataParser rawDataParser = new RawDataParser();

            // Displays an OpenFileDialog so the user can select a .dem file.  
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Demo files|*.dem";
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Select .dem file";

            // Show the Dialog.  
            // If the user clicked OK in the dialog and  
            // a .dem file was selected, open it.  
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string fileName in openFileDialog.FileNames)
                {
                    try
                    {
                        rawDataParser.ParseDemFile(fileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.ToString(), "Failed", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }

                // Success message
                MessageBox.Show("Finished parsing .dem file.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        /*
         * 
         */
        private void SeperateRawDataCSVIntoPlayers_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CSVParser csvParser = new CSVParser();

            // Displays an OpenFileDialog so the user can select a .csv file.  
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files|*.csv";
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Select a rawData.csv file";

            // Show the Dialog.  
            // If the user clicked OK in the dialog and  
            // a .csv file was selected, open it.  
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string fileName in openFileDialog.FileNames)
                {
                    try
                    {
                        csvParser.ParseCSVFileIntoPlayers(fileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.ToString(), "Failed", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }

                // Success message
                MessageBox.Show("Finished parsing rawData.csv file.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        /*
         * 
         */
        private void seperatePlayercsvIntoPathsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlayerPathParser playerPathParser = new PlayerPathParser();

            // Displays an OpenFileDialog so the user can select a .csv file.  
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files|*.csv";
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Select player#.csv files";

            // Show the Dialog.  
            // If the user clicked OK in the dialog and  
            // a .csv file was selected, open it.  
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string fileName in openFileDialog.FileNames)
                {
                    try
                    {
                        playerPathParser.ParsePlayerFileIntoPaths(fileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.ToString(), "Failed", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }

                // Success message
                MessageBox.Show("Finished parsing player#.csv files.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        /*
         * 
         */
        private void CombineMultipleCSVFilesIntoMasterCSV_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CombineCSVFiles combineCSVFiles = new CombineCSVFiles();

            // Displays an OpenFileDialog so the user can select a .csv file.  
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files|*.csv";
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Select path.csv files";

            // Show the Dialog.  
            // If the user clicked OK in the dialog and  
            // a .csv file was selected, open it.  
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Create the file and write the headers
                StreamWriter streamWriter = combineCSVFiles.CreateNewCSVFile("master");

                // For each file write the contents to the newly created file
                foreach (string fileName in openFileDialog.FileNames)
                {
                    try
                    {
                        // Add the contents of the fileName to the streamWriter
                        combineCSVFiles.AddNewCSVFile(fileName, streamWriter);
                    }
                    catch (Exception ex)
                    {
                        // Else failed message
                        MessageBox.Show("Error: " + ex.ToString(), "Failed", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
                
                streamWriter.Close();

                // Success message
                MessageBox.Show("Finished parsing path#.csv files.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        /*
         * 
         */
        private void ConnectToExistingDatabase_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Displays an OpenFileDialog so the user can select a .csv file.  
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "database files|*.db";
            openFileDialog.Title = "Select database.db file";

            // Show the Dialog.  
            // If the user clicked OK in the dialog and  
            // a .csv file was selected, open it.  
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_MasterTable = new MasterTable(openFileDialog.FileName, masterTableName);
            }
        }

        /*
         * 
         */
        private void CreateNewDatabaseFromMasterCSVFile_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_Database = new SQLDatabase(databaseName);

            m_MasterTable = m_Database.addMasterTable(masterTableName,
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
                "viewDirectionY double NOT NULL "
                );

            // Displays an OpenFileDialog so the user can select a .csv file.  
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files|*.csv";
            openFileDialog.Title = "Select master.csv file";

            // Show the Dialog.  
            // If the user clicked OK in the dialog and  
            // a .csv file was selected, open it.  
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (StreamReader stringReader = new StreamReader(openFileDialog.FileName))
                {
                    // First line is the column headers
                    string currentLine = stringReader.ReadLine();

                    m_MasterTable.BulkInsert(stringReader);
                }
            }
        }

        /*
         * 
         */
        private void CreateSuperVelocityCSVButton_Click(object sender, EventArgs e)
        {
            //for (int xValue = 0; xValue < 100; xValue++)
            for (int x = 0; x < Experiment.LevelAxisSubdivisions; x++)
            {
                // 
                string outputFileName = "velocityColumn." + x + ".csv";
                // and open it. 
                StreamWriter outputStream = new StreamWriter(outputFileName);

                Vector[,] result = m_MasterTable.SegmentDatabaseIntoArray(Experiment.LevelAxisSubdivisions, x);

                //for (int x = 0; x < Experiment.LevelAxisSubdivisions; x++)
                {
                    string line = "";

                    for (int y = 0; y < Experiment.LevelAxisSubdivisions; y++)
                    {
                        line += result[x, y].ToMyString() + ",";
                    }

                    outputStream.WriteLine(line);
                }

                outputStream.Close();
            }

            MessageBox.Show("Finished velocity csv.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        /*
         * 
         */
        private void Temp()
        {
            // Displays an OpenFileDialog so the user can select a .csv file.  
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files|*.csv";
            openFileDialog.Title = "Select path.csv file";

            // Show the Dialog.  
            // If the user clicked OK in the dialog and  
            // a .csv file was selected, open it.  
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (StreamReader stringReader = new StreamReader(openFileDialog.FileName))
                {

                    Vector lastKnownPosition;
                    Vector lastKnownVelocity;
                    Vector lastKnownAcceleration;
                    float lastKnownTime;
                    float predictionTime;

                    Vector currentVelocity;
                    Vector velocityTrend;
                    Vector accelerationTrend;

                    /*

                    experiment.Update(currentVelocity, velocityTrend);


                    Vector predictedDeadPosition = TraditionalDeadReckoning.Prediction(lastKnownPosition, lastKnownVelocity, lastKnownAcceleration, lastKnownTime, predictionTime);
                    
                    Vector predictedLedPosition = LedReckoning.Prediction(lastKnownPosition, lastKnownVelocity, lastKnownAcceleration, lastKnownTime, predictionTime, velocityTrend, accelerationTrend);
                    */
                }
            }
        }

        private void CombineVelCSVs_Click(object sender, EventArgs e)
        {
            // Displays an OpenFileDialog so the user can select a .csv file.  
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files|*.csv";
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Select path.csv files";

            // Show the Dialog.  
            // If the user clicked OK in the dialog and  
            // a .csv file was selected, open it.  
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Vector[,] returnArray = new Vector[Experiment.LevelAxisSubdivisions, Experiment.LevelAxisSubdivisions];

                string currentLine;

                // For each file write the contents to the newly created file
                foreach (string fileName in openFileDialog.FileNames)
                {
                    try
                    {
                        // Could just assume here and: x = int.Parse(fileName.Split('.')[1]);
                        if (int.TryParse(fileName.Split('.')[1], out int x))
                        {
                            using (StreamReader reader = new StreamReader(fileName))
                            {
                                while ((currentLine = reader.ReadLine()) != null)
                                {
                                    // Get each cell 
                                    string[] splitLine = currentLine.Split(',');

                                    for (int y = 0; y < Experiment.LevelAxisSubdivisions; y++)
                                    {
                                        // Get each value
                                        string[] splitCell = splitLine[y].Split(new string[] { "@value" }, StringSplitOptions.RemoveEmptyEntries);
                                        returnArray[x, y] = new Vector(splitCell[0], splitCell[1], splitCell[2]);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Else failed message
                        MessageBox.Show("Error: " + ex.ToString(), "Failed", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }

                string outputFileName = "masterVelocity" + Experiment.LevelAxisSubdivisions + ".csv";
                // and open it. 
                StreamWriter outputStream = new StreamWriter(outputFileName);

                for (int y = 0; y < Experiment.LevelAxisSubdivisions; y++)
                {
                    string outputRow = "";

                    for (int x = 0; x < Experiment.LevelAxisSubdivisions; x++)
                    {
                        outputRow += returnArray[x, y].ToMyEasierSplitString() + ",";
                    }

                    outputStream.WriteLine(outputRow);
                }

                outputStream.Close();

                // Success message
                MessageBox.Show("Finished master velocity file!!!.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void LoadLedReckoningMasterCSVButton_Click(object sender, EventArgs e)
        {
            // Displays an OpenFileDialog so the user can select a .csv file.  
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files|*.csv";
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Select path.csv files";

            // Show the Dialog.  
            // If the user clicked OK in the dialog and  
            // a .csv file was selected, open it.  
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string currentLine;

                // For each file write the contents to the newly created file
                foreach (string fileName in openFileDialog.FileNames)
                {
                    try
                    {
                        using (StreamReader reader = new StreamReader(fileName))
                        {
                            int y = 0;

                            while ((currentLine = reader.ReadLine()) != null)
                            {
                                // Get each cell 
                                string[] splitLine = currentLine.Split(',');

                                for (int x = 0; x < Experiment.LevelAxisSubdivisions; x++)
                                {
                                    // Get each value
                                    string[] splitCell = splitLine[x].Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries);
                                    ledReckoningLevelDataTable[x, y] = new Vector(splitCell[0], splitCell[1], splitCell[2]);
                                }

                                y++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Else failed message
                        MessageBox.Show("Error: " + ex.ToString(), "Failed", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }

                // Success message
                MessageBox.Show("Finished importing level data csv file.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);



                Bitmap returnBMP = new Bitmap(1080, 1080);
                Graphics returnGraphics = Graphics.FromImage(returnBMP);





                float xStep = 1080.0f/*(float)VectorMapPictureBox.Width*/ / Experiment.LevelAxisSubdivisions;
                float yStep = 1080.0f/*(float)VectorMapPictureBox.Height*/ / Experiment.LevelAxisSubdivisions;
                float maxSize = InfernoLevelData.maxPlayerSpeed;

                for (int x = 0; x < Experiment.LevelAxisSubdivisions; x++)
                {
                    for (int y = 0; y < Experiment.LevelAxisSubdivisions; y++)
                    {
                        // Draw the map the right way around, reverse the 0-99 y value
                        Vector velocityTrend = ledReckoningLevelDataTable[x, Experiment.LevelAxisSubdivisions-y-1];

                        // Only draw a line if there is a non zero vector there
                        if (velocityTrend.Length != 0.0)
                        {
                            // Set the colour as a grayscale currently ish (/2)
                            Color lineColour = Color.FromArgb(
                                128,
                                (int)velocityTrend.Length / 2,
                                (int)velocityTrend.Length / 2,
                                (int)velocityTrend.Length / 2);

                            // Create the pen
                            Pen pen = new Pen(lineColour, 1.0f);

                            // Make the line look pretty
                       //     pen.StartCap = System.Drawing.Drawing2D.LineCap.RoundAnchor;
                            pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

                            // Start drawing from the centre of the tile
                            float startX = (x + 0.5f) * xStep;
                            float startY = (y + 0.5f) * yStep;

                            // * Step / (2.0f * maxSize) limits the draw to the edge of the tile
                            float endX = startX + (velocityTrend.X * xStep / (0.1f * maxSize));
                            float endY = startY + (velocityTrend.Y * yStep / (0.1f * maxSize));

                            // Draw it
                            returnGraphics.DrawLine(pen, startX, startY, endX, endY);

                            // Prob need to dispose the pen
                        }
                    }
                }

                returnBMP.Save("visualisation.png", System.Drawing.Imaging.ImageFormat.Png);

                VectorMapPictureBox.Image = returnBMP;
                

                // Success message
                MessageBox.Show("What did you see old man.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void PerformExperimentButton_Click(object sender, EventArgs e)
        {
            // Displays an OpenFileDialog so the user can select a .csv file.  
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files|*.csv";
            openFileDialog.Title = "Select path.csv file";

            // Show the Dialog.  
            // If the user clicked OK in the dialog and  
            // a .csv file was selected, open it.  
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                PlayerPathLoader playerPath = new PlayerPathLoader(openFileDialog.FileName);

                Experiment experiment = new Experiment(playerPath, ledReckoningLevelDataTable);
            }
        }

        private void CheckMapNamesOfdemSamples_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RawDataParser rawDataParser = new RawDataParser();
            Dictionary<string, string> mapNameDictionary = new Dictionary<string, string>();

            // Displays an OpenFileDialog so the user can select a .dem file.  
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Demo files|*.dem";
            openFileDialog.Multiselect = true;
            openFileDialog.Title = "Select .dem files";

            // Show the Dialog.  
            // If the user clicked OK in the dialog and  
            // a .dem file was selected, open it.  
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string fileName in openFileDialog.FileNames)
                {
                    string mapName = rawDataParser.GetMapName(fileName);

                    mapNameDictionary.Add(fileName, mapName);
                }
            }

            string outputFileName = "mapNameDict.csv";
            // and open it. 
            StreamWriter outputStream = new StreamWriter(outputFileName);

            foreach (string game in mapNameDictionary.Keys)
            {
                outputStream.WriteLine(game + "," + mapNameDictionary[game]);
            }

            outputStream.Close();

            // Success message
            MessageBox.Show("Finished mapNameDictFile.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        /*
         * 
         */
        private void QueryMinMaxPositionValues_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double[] maxMin = m_MasterTable.GetMaxMin();

            MessageBox.Show("minX = " + maxMin[0] + ", maxX = " + maxMin[1] + ", minY = " + maxMin[2] + ", maxY = " + maxMin[3], "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {
            string helpMessage =
                "Start by loading a .dem file \n" +
                "The resulting rawData.csv should then be parsed into player paths. \n" +
                "This can be used to prune empty or non existent paths such as";

            MessageBox.Show(helpMessage, "Help", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }


        /*
         * Still need to:
         * 
         * -- write a function that takes in the csv file on start and stores in an array (? or better data structure)
         * - Write a function that takes in the actual position, and returns the 0-Experiment.LevelAxisSubdivisions (x, y) coords for look up
         * - Then in LedReckoning.cs look up the trend
         * 
         * - Do stuff with acceleration
         * 
         * - Write the experiment:
         *      - Need to update
         *      - Choose random path files? Random start and end points
         *      -- Shouldn't be a zero distance path. No point
         *      - Output comparison results
         *      
         *      - incorporate threshold stuff. Seperate experiment?
         *      - Like maybe a path long experiment that tracks a frame by frame displacement
         *      - incorporate std dead reckoning protocol like update after 5 secs etc
         *      
         * - Make the damn winforms look better
         * 
         */
    }
}
