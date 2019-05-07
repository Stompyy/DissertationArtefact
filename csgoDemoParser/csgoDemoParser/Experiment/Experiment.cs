using System;
using System.IO;
using System.Windows.Forms;

namespace csgoDemoParser
{
    /*
     * Used to hold infomation for drawing the path data
     * 
     * Includes the drawing position and whether a packet update occurred
     */
    struct VisualisationData
    {
        // The render coordinates for drawing
        public float X, Y;

        // Whether a packet update occured at this position. Reflected in the visualisation
        public bool packetUpdate;

        /*
         * Constructor with packet update information
         */
        public VisualisationData (float _x, float _y, bool _packetUpdated)
        {
            this.X = _x;
            this.Y = _y;
            this.packetUpdate = _packetUpdated;
        }

        /*
         * Constructor with no packet update infomation
         */
        public VisualisationData(float _x, float _y)
        {
            this.X = _x;
            this.Y = _y;
            this.packetUpdate = false;
        }
    }

    /*
     * Handles performing an experiment upon a path using dead reckoning and led reckoning simulations
     */
    class Experiment
    {
        // The positions of these values in a comma seperated split list from a playerPath.csv file ReadLine()
        const int listPositionX = 2;
        const int listPositionY = 3;
        const int listPositionZ = 4;
        const int listVelocityX = 8;
        const int listVelocityY = 9;
        const int listVelocityZ = 10;

        // The data needed to draw the paths in the visualiser after the experiment
        public VisualisationData[] actualPositions, deadReckonedPositions, ledReckonedPositions;

        // The number of subdivisions per axis of the implied overlay grid on the level, from which velocity trends are averaged within
        // A value of 100 will result in a grid 100 * 100 for 10,000 potential grid squares
        public const int LevelAxisSubdivisions = 100;

        // The frames per second of the demo. Used to convert map units per second to map units per frame
        public const float framesPerSecond = 60.0f;

        // The given thresholds for the prediction algorithms
        const double traditionalDRThreshold = 20.0;

        // These are required to have the traditionalDRThreshold value as the median value to remain relevant
        const double minimumAllowedThreshold = 5.0;
        const double maximumAllowedThreshold = 35.0;

        // The player path on which the experiment will be carried out
        private PlayerPathLoader m_PlayerPath;

        // The led reckoning velocity trend level data that the led reckoning simulation uses
        private Vector[,] m_LedReckoningLevelDataTable;

        // The simulation algorithms
        public LedReckoning m_LedReckoning;
        public TraditionalDeadReckoning m_TraditionalDeadReckoning;

        // The running totals of distance from simulation position from actual position
        public double totalLedReckoningDistanceFromActual, totalDeadReckoningDistanceFromActual;

        // The Vectors used to carry frame update information
        public Vector initialPosition, currentPosition, initialVelocity, currentVelocity, previousVelocity;

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

            // Initialise the Visualisation data for each simulation
            actualPositions = new VisualisationData[m_PlayerPath.pathDictionary.Count];
            deadReckonedPositions = new VisualisationData[m_PlayerPath.pathDictionary.Count];
            ledReckonedPositions = new VisualisationData[m_PlayerPath.pathDictionary.Count];

            // Get the first value for the initial position
            string[] frameInfo = m_PlayerPath.pathDictionary[0];
            initialPosition = new Vector(frameInfo[listPositionX], frameInfo[listPositionY], frameInfo[listPositionZ]);
            initialVelocity = new Vector(frameInfo[listVelocityX], frameInfo[listVelocityY], frameInfo[listVelocityZ]);

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

            // Write the headers for the results CSV file
            outputStream.WriteLine("PathName," +
                "NumFrames," +
                "Average dead reckoned error distance," +
                "DeadUpdates," +
                "Average led reckoned error distance," +
                "LedUpdates"); 
            
            //------------------
            // Experiment
            //------------------

            // Start with a packet update as would happen in a real scenario
            // This could be handled with the prediction algorithm constructor but we are simulating proper use
            // Simulation begins with zero acceleration at frame zero, initial velocity is normally zero except for test cases involving continuous velocity
            m_TraditionalDeadReckoning.SimulatePacketUpdate(
                initialPosition,
                initialVelocity,
                new Vector(0.0f),
                0
                );

            m_LedReckoning.SimulatePacketUpdate(
                initialPosition,
                initialVelocity,
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
                // and updates the rival prediction algorithms are returning. Swap the below lines for the frame data to be written 
                // into the output stream

         //       outputStream.WriteLine(Update());
                Update();

                // Advance the simulation
                currentFrame++;
            }

            // Once the path is finished, write the comparison information
            outputStream.WriteLine($"{pathLoader.pathName}," +
                $"{pathLoader.pathDictionary.Count.ToString()}," +
                $"{totalDeadReckoningDistanceFromActual / pathLoader.pathDictionary.Count}," +
                $"{m_TraditionalDeadReckoning.numberOfUpdatesNeeded}," +
                $"{totalLedReckoningDistanceFromActual / pathLoader.pathDictionary.Count}," +
                $"{m_LedReckoning.numberOfUpdatesNeeded}"
                );

            // Close the output stream
            outputStream.Close();
        }

        /*
         * Updates the simulations according to the currentFrame
         * 
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

            // Used in the visualisation data
            bool dRPacketUpdatePrompted = false;

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

                // Simulate a packet update
                m_TraditionalDeadReckoning.SimulatePacketUpdate(
                    currentPosition,
                    currentVelocity,
                    currentAcceleration,
                    currentFrame
                    );

                // Debug message monitors updates in the return string
                debug += (" dead ");

                // Used in the visualisation data
                dRPacketUpdatePrompted = true;
            }

            //-----------------
            // Led Reckoning
            //-----------------

            // Used in the visualisation data
            bool lRPacketUpdatePrompted = false;

            // Get the led reckoned position
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

                // Simulate a packet update
                m_LedReckoning.SimulatePacketUpdate(
                    currentPosition,
                    currentVelocity,
                    currentAcceleration,
                    currentFrame
                    );

                // Debug message monitors updates in the return string
                debug += " led ";

                // Used in the visualisation data
                lRPacketUpdatePrompted = true;
            }

            // Additional debug information
            debug += "," + m_LedReckoning.Threshold.ToString();

            // Update the visualisation data with this frame's simulation positions
            actualPositions[currentFrame] = new VisualisationData(currentPosition.X, currentPosition.Y, false);
            deadReckonedPositions[currentFrame] = new VisualisationData(deadReckonedPosition.X, deadReckonedPosition.Y, dRPacketUpdatePrompted);
            ledReckonedPositions[currentFrame] = new VisualisationData(ledReckonedPosition.X, ledReckonedPosition.Y, lRPacketUpdatePrompted);

            // Return the return string
            return currentFrame + "," +
                currentPosition.ToMyEasierSplitString() + "," +
                deadReckonedPosition.ToMyEasierSplitString() + "," +
                ledReckonedPosition.ToMyEasierSplitString() + "," +
                debug;
        }
    }
}
