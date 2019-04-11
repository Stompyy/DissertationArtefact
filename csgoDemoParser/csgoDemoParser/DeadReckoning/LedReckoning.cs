using System;
using System.IO;
using System.Windows.Forms;

namespace csgoDemoParser
{
    /*
     * Level Experiential/Educated Dead (Led) Reckoning
     */
    class LedReckoning : TraditionalDeadReckoning
    {
        // The minimum and maximum distance values for threshold based packet update requests
        public double MinimumThreshold, MaximumThreshold;

        // The current velocity trend for the simulations position
        private Vector velocityTrend;

        // The data table from which to look up the the current velocity trend
        private Vector[,] m_LedReckoningLevelDataTable;

        // The -1 to 1 compliance value based off of the dot product between the current velocity and the velocity trend
        private double compliance;

        // The 0 to 1 confidence value is the absolute value of the compliance. 0 = perpendicular, 1 = parallel. Defined as |compliance|
        private double confidence;

        /*
         * Constructor references the led reckoning level data table
         */
        public LedReckoning(Vector initialPosition, Vector[,] ledReckoningLevelDataTable) : base(initialPosition)
        {
            // Set the reference to the velocity trend data
            m_LedReckoningLevelDataTable = ledReckoningLevelDataTable;

            // Initialise trend to zero for the first 
            velocityTrend = new Vector(0.0f);
        }

        /*
         * This threshold is calculated with accordance to the current velocity trend.
         */
        public new double Threshold
        { get
            {
                // Should this take magnitude into account also... areas of near zero overall velocity trend should not as be subject
                // Vector length is the velocity so just 
                //velocityTrend.Length / InfernoLevelData.maxPlayerSpeed

                // Get the current velocity trend
                velocityTrend = GetVelocityTrend(deadReckonedPosition.X, deadReckonedPosition.Y);
                
                // If there is a simulated velocity value to work with (be able to take a dot product from)
                if (simulatedVelocity.Length != 0.0 && velocityTrend.Length != 0.0f)
                {
                    // Update the compliance and confidence values
                    compliance = Vector.Dot(simulatedVelocity.Normalised, velocityTrend.Normalised);
                    confidence = Math.Abs(compliance);
                }
                else
                {
                    // Default confidence is the uncommitted midpoint between 0 - 1.
                    confidence = 0.5f;
                }

                // Return the max at full confidence, min at zero, and a grade throughout for an intermediate value
                return MinimumThreshold + (MaximumThreshold - MinimumThreshold) * confidence;
            }
        }

        /*
         * Uses Newtonian motion equation to get a simulated position based off the last known position
         */
        protected override Vector GetProjectedPosition(Vector startingPosition, Vector velocity, float deltaTime)
        {
            // I think this may need to look at the actual changes in velocity not the velocity trend
            // Adjust the acceleration in line with the velocity trend data
            // Attempts to push the simulation towards the velocity trend by finding the average of the last known acceleration and the velocityTrend compliance
            Vector accelerationTrend = (lastKnownAcceleration + velocityTrend * compliance) / 2.0f; //

            // Second order derivitive prediction using Newtonian laws of motion
            return startingPosition + (velocity * deltaTime) + (accelerationTrend * 0.5f * deltaTime * deltaTime);
        }

        /*
         * Takes level position values, and returns the velocity trend found for that position in the LedReckoning level data
         */
        private Vector GetVelocityTrend(double playerPositionX, double playerPositionY)
        {
            // Translate the world position into appropriate data structure look up coordinates
            int[] lookUpCoords = InfernoLevelData.TranslatePositionIntoLookUpCoordinates(playerPositionX, playerPositionY);

            //  Look at the rounded down int value
            Vector lowerBound = m_LedReckoningLevelDataTable[lookUpCoords[0], lookUpCoords[1]];

            // Look at the rounded up int value
            Vector upperBound = m_LedReckoningLevelDataTable[lookUpCoords[0] + 1, lookUpCoords[1] + 1];

            // Get the x position place in the grid tile as a 0-1 value across the tile
            double xStep = (playerPositionX - (InfernoLevelData.minimumXValue + lookUpCoords[0] * InfernoLevelData.subdivisionSizeX)) / InfernoLevelData.subdivisionSizeX;


            // Get the y position place in the grid tile as a 0-1 value across the tile
            double yStep = (playerPositionY - (InfernoLevelData.minimumYValue + lookUpCoords[1] * InfernoLevelData.subdivisionSizeY)) / InfernoLevelData.subdivisionSizeY;

            // Use the 0-1 placement within the grid values to smoothstep (bell shape interpolation) between the lower and upper bounds
            return new Vector()
            {
                X = lowerBound.X + (upperBound.X - lowerBound.X) * Utilities.FSmoothStep(xStep),
                Y = lowerBound.Y + (upperBound.Y - lowerBound.Y) * Utilities.FSmoothStep(yStep),
                Z = lowerBound.Z
            };
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
            // If the user clicked OK in the dialog and  
            // a .csv file was selected, open it.  
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    // Open the fileName
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
