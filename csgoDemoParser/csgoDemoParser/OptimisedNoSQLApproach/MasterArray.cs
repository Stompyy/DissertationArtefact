using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace csgoDemoParser
{
    /*
     * Used to accumulate Vectors and find the mean average by using the count
     */
    struct AreaVelocityInfo
    {
        public Vector accumulativeVelocity;
        public int count;
    }

    /*
     * Holds and manages the velocity Vector trend data structure
     */
    class MasterArray
    {
        // Declare data structure here
        public Vector[,] averageVelocityTable;

        /*
         * Constructor
         */
        public MasterArray()
        {
            // Initialise the data structure
            averageVelocityTable = new Vector[Experiment.LevelAxisSubdivisions, Experiment.LevelAxisSubdivisions];

            // Prompts the choosing of path.CSV files and populates the data structure
            AccumulateVelocity();
        }

        /*
         * Loads a single 'master.csv' file of all paths accumulated, and processes each line into the data structure
         */
        private void AccumulateVelocity()
        {
            // Displays an OpenFileDialog so the user can select a .csv file.  
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV file|*.csv";
            openFileDialog.Title = "Select master.csv file";

            // Show the Dialog. If the user clicked OK in the dialog and a .db file was selected, open it.  
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Disposable string reader for easy memory clean up
                using (StreamReader stringReader = new StreamReader(openFileDialog.FileName))
                {
                    // For benchmarking and optimisation as detailed in a blog post here:
                    // https://journal.falmouth.ac.uk/rs195256optimisation/2019/?order=asc
                    Timer timer = new Timer();
                    timer.Start();

                    // Keep to this local scope so that garbage collector can dispose of it when out of scope
                    AreaVelocityInfo[,] cumalativeVelocityTable = new AreaVelocityInfo[Experiment.LevelAxisSubdivisions, Experiment.LevelAxisSubdivisions];
                    
                    // Initialise the data structure values ready for accumulating values
                    for (int x = 0; x < Experiment.LevelAxisSubdivisions; x++)
                    {
                        for (int y = 0; y < Experiment.LevelAxisSubdivisions; y++)
                        {
                            // Set zero values
                            cumalativeVelocityTable[x, y].accumulativeVelocity = new Vector(0.0f);
                            cumalativeVelocityTable[x, y].count = 0;
                        }
                    }
                    
                    // First line is the column headers - ignore
                    stringReader.ReadLine();

                    // Used in parsing the CSV lines
                    string currentLine;
                    string[] lineItems;

                    // For debugging purposes if encountering corrupted data
                    int acceptedLines = 0;
                    int rejectedLines = 0;

                    // CurrentLine will be null when the StreamReader reaches the end of file
                    while ((currentLine = stringReader.ReadLine()) != null)
                    {
                        // Create an array of the comma seperated string items
                        lineItems = currentLine.Split(',');

                        // positionX, positionY from the csv structure as outlined in CSVWriter.cs
                        // Translates the position into the valid data structure indices
                        int[] lookUpCoords = InfernoLevelData.TranslatePositionIntoLookUpCoordinates(double.Parse(lineItems[2]), double.Parse(lineItems[3]));

                        // Try catch here in case of incomplete CSV lines
                        try
                        {
                            // Add the velocity values from this line to the accumulating Velocity value for the data structures relevant index
                            cumalativeVelocityTable[lookUpCoords[0], lookUpCoords[1]].accumulativeVelocity += new Vector(lineItems[8], lineItems[9], lineItems[10]);

                            // And increment the count for that data structure index
                            cumalativeVelocityTable[lookUpCoords[0], lookUpCoords[1]].count++;
                            acceptedLines++;
                        }
                        catch
                        {
                            rejectedLines++;
                        }
                    }

                    // Stop the timer
                    timer.Stop();

                    // Success message
                    MessageBox.Show("Finished cumulating .csv file. " + timer.GetTimeElapsed() + " MS. " +
                        "Accepted: " + acceptedLines + ". " +
                        "Rejected: " + rejectedLines,
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                    // Use the count in each AreaVelocityInfo to calculate the mean average
                    // Use this to populate the main member data structure cumalativeVelocityTable
                    for (int x = 0; x < Experiment.LevelAxisSubdivisions; x++)
                    {
                        for (int y = 0; y < Experiment.LevelAxisSubdivisions; y++)
                        {
                            // Check for divide by zero case
                            if (cumalativeVelocityTable[x, y].count != 0)
                            {
                                // Find the mean average
                                averageVelocityTable[x, y] = cumalativeVelocityTable[x, y].accumulativeVelocity / cumalativeVelocityTable[x, y].count;
                            }
                            else
                            {
                                // Else set as zero value
                                averageVelocityTable[x, y] = new Vector(0.0f);
                            }
                        }
                    }
                }
                
                // Success message
                MessageBox.Show("Finished averaging .csv file.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        /*
         * Allows the user to choose and load a led reckoning velocity trend .csv into memory as a Vector[,]
         */
        public static Vector[,] LoadLedReckoningData()
        {
            Vector[,] returnArray = new Vector[Experiment.LevelAxisSubdivisions, Experiment.LevelAxisSubdivisions];

            // Displays an OpenFileDialog so the user can select a .csv file.  
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV file|*.csv";
            openFileDialog.Title = "Select master.csv file";

            // Show the Dialog.  
            // If the user clicked OK in the dialog and a .csv file was selected, open it.
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    // Open the file selected
                    using (StreamReader reader = new StreamReader(openFileDialog.FileName))
                    {
                        // Start at y = 0 row
                        int y = 0;

                        string currentLine;

                        // ReadLine() is null at the end of the file
                        while ((currentLine = reader.ReadLine()) != null)
                        {
                            // Get each cell 
                            string[] splitLine = currentLine.Split(',');

                            // Step through the split line parsing then writing each cell as a Vector into the returnArray
                            for (int x = 0; x < Experiment.LevelAxisSubdivisions; x++)
                            {
                                // Split the cell by the chosen non comma seperator, in this case '@'
                                string[] splitCell = splitLine[x].Split(new string[] { "@" }, StringSplitOptions.RemoveEmptyEntries);

                                // Construct the Vector and set it's place in the returnArray
                                returnArray[x, y] = new Vector(splitCell[0], splitCell[1], splitCell[2]);
                            }

                            // Increment y value to look at next row
                            y++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Else failed message
                    MessageBox.Show("Error: " + ex.ToString(), "Failed", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }

                // Success message
                MessageBox.Show("Finished importing level data csv file.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

            return returnArray;
        }
    }
}
