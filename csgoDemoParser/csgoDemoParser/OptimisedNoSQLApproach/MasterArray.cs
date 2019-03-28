using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace csgoDemoParser
{
    enum BelongingTeam
    {
        terrorist, counterTerrorist
    }

    struct FrameInfo
    {
        public BelongingTeam team;
        public Vector position;
        public Vector velocity;
        // Any other info needed? Not even using BelongingTeam yet?
    }

    struct AreaVelocityInfo
    {
        public Vector cumulativeVelocity;
        public int count;
    }

    class MasterArray
    {
        // Declare data structure here
        public Vector[,] averageVelocityTable;

        public MasterArray()
        {
            averageVelocityTable = new Vector[Experiment.LevelAxisSubdivisions, Experiment.LevelAxisSubdivisions];

            //LoadCSVIntoMemory();
            CumulateVelocity();
        }

        private void LoadCSVIntoMemory()
        {
            // Displays an OpenFileDialog so the user can select a .csv file.  
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV file|*.csv";
            openFileDialog.Title = "Select master.csv file";

            // Show the Dialog. If the user clicked OK in the dialog and a .db file was selected, open it.  
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                // Disposable string reader for easy clean up
                using (StreamReader stringReader = new StreamReader(openFileDialog.FileName))
                {

                }
            }
        }

        private void CumulateVelocity()
        {
            // Displays an OpenFileDialog so the user can select a .csv file.  
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV file|*.csv";
            openFileDialog.Title = "Select master.csv file";

            // Show the Dialog. If the user clicked OK in the dialog and a .db file was selected, open it.  
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                // Disposable string reader for easy clean up
                using (StreamReader stringReader = new StreamReader(openFileDialog.FileName))
                {

                    Timer timer = new Timer();
                    timer.Start();

                    // Keep to this local scope so that garbage collector can dispose of it when out of scope (hopefully)
                    AreaVelocityInfo[,] cumalativeVelocityTable = new AreaVelocityInfo[Experiment.LevelAxisSubdivisions, Experiment.LevelAxisSubdivisions];
                    
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



                    // First line is the column headers - ignore
                    stringReader.ReadLine();

                    string currentLine;
                    string[] lineItems;

                    int acceptedLines = 0;
                    int rejectedLines = 0;

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

                    timer.Stop();

                    // Success message
                    MessageBox.Show("Finished cumulating .csv file. " + timer.GetTimeElapsed() + " MS. " +
                        "Accepted: " + acceptedLines + ". " +
                        "Rejected: " + rejectedLines,
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
                                averageVelocityTable[x, y] = cumalativeVelocityTable[x, y].cumulativeVelocity / cumalativeVelocityTable[x, y].count;
                            }
                            else
                            {
                                averageVelocityTable[x, y] = new Vector(0.0f);
                            }
                        }
                    }
                }
                
                // Success message
                MessageBox.Show("Finished averaging .csv file.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
    }
}
