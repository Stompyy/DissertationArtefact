using System;
using System.IO;
using System.Windows.Forms;

namespace csgoDemoParser
{
    struct VisualisationData
    {
        public float X, Y;

        public VisualisationData (float _x, float _y)
        {
            this.X = _x;
            this.Y = _y;
        }
    }

    class Experiment
    {
        public VisualisationData[] actualPositions, deadReckonedPositions, ledReckonedPositions;

        // The positions of these values in a comma seperated split list from a playerPath.csv file ReadLine()
        const int listPositionX = 2;
        const int listPositionY = 3;
        const int listPositionZ = 4;
        const int listVelocityX = 8;
        const int listVelocityY = 9;
        const int listVelocityZ = 10;

        // The number of subdivisions per axis of the implied overlay grid on the level, from which velocity trends are averaged within
        // A value of 100 will result in a grid 100 * 100 for 10,000 potential grid squares
        public const int LevelAxisSubdivisions = 100;

        // The frames per second of the demo. Used to convert map units per second to map units per frame
        public const float framesPerSecond = 60.0f;

        // The given thresholds for the prediction algorithms - These will be required to be investigated for best results
        const double traditionalDRThreshold = 100.0;
        const double minimumAllowedThreshold = 90.0;
        const double maximumAllowedThreshold = 110.0;

        // The player path on which the experiment will be carried out
        private PlayerPathLoader m_PlayerPath;

        // The led reckoning velocity trend level data that the led reckoning simulation uses
        private Vector[,] m_LedReckoningLevelDataTable;

        // The simulation algorithms
        private LedReckoning m_LedReckoning;
        private TraditionalDeadReckoning m_TraditionalDeadReckoning;

        // The running totals of distance from simulation position from actual position
        private double totalLedReckoningDistanceFromActual, totalDeadReckoningDistanceFromActual;

        // The Vectors used to carry frame update information
        private Vector currentPosition, currentVelocity, previousVelocity;

        // The name of the path and experiment
        public string experimentName;

        // The current frame of the simulation
        private int currentFrame;

        /*
         * Constructor of the experiment class triggers an experiment with the loaded data
         */
        public Experiment(PlayerPathLoader pathLoader, Vector[,] ledReckoningLevelDataTable)
        {
            // Set the local references from the constructor arguments
            m_LedReckoningLevelDataTable = ledReckoningLevelDataTable;
            m_PlayerPath = pathLoader;

            actualPositions = new VisualisationData[m_PlayerPath.pathDictionary.Count];
            deadReckonedPositions = new VisualisationData[m_PlayerPath.pathDictionary.Count];
            ledReckonedPositions = new VisualisationData[m_PlayerPath.pathDictionary.Count];

            // get the first value for the initial position
            string[] frameInfo = m_PlayerPath.pathDictionary[0];
            Vector initialPosition = new Vector(frameInfo[listPositionX], frameInfo[listPositionY], frameInfo[listPositionZ]);

            // Initialise the traditional dead reckoning prediction algorithm and set the threshold value
            m_TraditionalDeadReckoning = new TraditionalDeadReckoning(initialPosition)
            {
                Threshold = traditionalDRThreshold
            };

            // Initialise the the led reckoning prediction algorithm and set the min/max threshold values
            m_LedReckoning = new LedReckoning(initialPosition, ledReckoningLevelDataTable)
            {
                MinimumThreshold = minimumAllowedThreshold,
                MaximumThreshold = maximumAllowedThreshold
            };


            // Create the fileName. Get the filename from the fileName address
            string[] filePath = pathLoader.pathName.Split(new[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
            // Get the last string item
            experimentName = filePath[filePath.Length - 1];
            // Create the full filename
            string outputFileName = "Experiment." + experimentName;
            // and open it. 
            StreamWriter outputStream = new StreamWriter(outputFileName);

            // Write some info on the expexperimeee
            outputStream.WriteLine("Path: " + pathLoader.pathName + 
                ",Number of frames: " + pathLoader.pathDictionary.Count.ToString() + 
                ". Path time: " + pathLoader.pathDictionary.Count/Experiment.framesPerSecond + " seconds.");

            // Write the column headers
            outputStream.WriteLine("frame,actual,traditional,led,update required,LedReckoning threshold");

            //------------------
            // Experiment
            //------------------

            // Start with a packet update as would happen in a real scenario
            // This could be handled with the prediction algorithm constructor but we are simulating proper use
            m_TraditionalDeadReckoning.SimulatePacketUpdate(
                initialPosition,
                new Vector(0.0f),
                new Vector(0.0f),
                0
                );

            m_LedReckoning.SimulatePacketUpdate(
                initialPosition,
                new Vector(0.0f),
                new Vector(0.0f),
                0
                );

            // Start at the beginning of the path
            currentFrame = 0;

            // Set the accuracy running totals to zero at start
            totalLedReckoningDistanceFromActual = 0.0;
            totalDeadReckoningDistanceFromActual = 0.0;

            // Simulate a tick update by advancing a frame count and calling Update()
            while (currentFrame < m_PlayerPath.pathDictionary.Count)
            {
                // Update returns a string which is not necessary to write to the output stream, but interesting to see what positions
                // and updates the rival prediction algorithms are returning.
                outputStream.WriteLine(
                    Update()
                    );

                // Advance the simulation
                currentFrame++;
            }

            // Finally write the comparison information
            outputStream.WriteLine(
                "Average dead reckoned error distance: " + totalDeadReckoningDistanceFromActual / pathLoader.pathDictionary.Count +
                " needing " + m_TraditionalDeadReckoning.numberOfUpdatesNeeded + " updates. " +

                "Average led reckoned error distance: " + totalLedReckoningDistanceFromActual / pathLoader.pathDictionary.Count +
                " needing " + m_LedReckoning.numberOfUpdatesNeeded + " updates. "
                );

            // Close the output stream
            outputStream.Close();

            // Success message
            MessageBox.Show("Finished Experiment: " + experimentName, "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        /*
         * Updates the simulations according to the currentFrame
         * Returns a string detailing the currentFrame, actual, dead reckoned and led reckoned positions, and any debug message.
         */
        private string Update()
        {
            // Initialise an empty string used for debug messages
            string debug = "";

            // Set the previousVelocity value as the currentVelocity to preserve it for acceleration comparison
            previousVelocity = currentVelocity;

            // Retrieve the frame information from the path dictionary for the currentFrame
            string[] frameInfo = m_PlayerPath.pathDictionary[currentFrame];

            // Get the currentPosition and the currentVelocity from the frame information
            currentPosition = new Vector(frameInfo[listPositionX], frameInfo[listPositionY], frameInfo[listPositionZ]);
            currentVelocity = new Vector(frameInfo[listVelocityX], frameInfo[listVelocityY], frameInfo[listVelocityZ]);

            //-----------------
            // Dead Reckoning
            //-----------------

            // Get the dead reckoned position
            Vector deadReckonedPosition = m_TraditionalDeadReckoning.GetDeadReckonedPositionWithVelocityBlending(currentFrame);

            // Find the distance of the simulated position from the actual position
            double deadReckonedPosDistanceFromActual = (currentPosition - deadReckonedPosition).Length;

            // Add to the running count as an ultimate measure of accuracy
            totalDeadReckoningDistanceFromActual += deadReckonedPosDistanceFromActual;

            // Check against threshold to decide whether to prompt a packet update
            if (deadReckonedPosDistanceFromActual > m_TraditionalDeadReckoning.Threshold)
            {
                // Request dead reckoning packet update
                // Calculate current acceleration as change in velocity divide by time
                Vector currentAcceleration = (currentVelocity - previousVelocity) / (1.0f / Experiment.framesPerSecond);

                m_TraditionalDeadReckoning.SimulatePacketUpdate(
                    currentPosition,
                    currentVelocity,
                    currentAcceleration,
                    currentFrame
                    );

                // Debug message monitors updates in the csv
                debug += "dead ";
            }

            //-----------------
            // Led Reckoning
            //-----------------

            // Get  the led reckoned position
            Vector ledReckonedPosition = m_LedReckoning.GetDeadReckonedPositionWithVelocityBlending(currentFrame);

            // Find the distance of the simulated position from the actual position
            double ledReckonedPosDistanceFromActual = (currentPosition - ledReckonedPosition).Length;

            // Add to the running count as an ultimate measure of accuracy
            totalLedReckoningDistanceFromActual += ledReckonedPosDistanceFromActual;
            
            // Check against threshold to decide whether to prompt a packet update
            if (ledReckonedPosDistanceFromActual > m_LedReckoning.Threshold)
            {
                // Request Led Reckoning packet update
                // Calculate current acceleration as change in velocity divide by time
                Vector currentAcceleration = (currentVelocity - previousVelocity) / (1.0f / Experiment.framesPerSecond);

                m_LedReckoning.SimulatePacketUpdate(
                    currentPosition,
                    currentVelocity,
                    currentAcceleration,
                    currentFrame
                    );

                // Debug message monitors updates in the csv
                debug += "led ";
            }

            // Additional debug information - need to fine tune the minimum and maximum values for the LedReckoning threshold
            debug += "," + m_LedReckoning.Threshold.ToString();

            actualPositions[currentFrame] = new VisualisationData(currentPosition.X, currentPosition.Y);
            deadReckonedPositions[currentFrame] = new VisualisationData(deadReckonedPosition.X, deadReckonedPosition.Y);
            ledReckonedPositions[currentFrame] = new VisualisationData(ledReckonedPosition.X, ledReckonedPosition.Y);

            // Construct the return string
            return currentFrame + "," +
                currentPosition.ToMyEasierSplitString() + "," +
                deadReckonedPosition.ToMyEasierSplitString() + "," +
                ledReckonedPosition.ToMyEasierSplitString() + "," +
                debug;
        }
    }
}
