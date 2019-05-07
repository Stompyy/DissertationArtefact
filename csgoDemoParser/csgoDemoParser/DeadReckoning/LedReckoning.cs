using System;

namespace csgoDemoParser
{
    /*
     * Level Experiential Dead (Led) Reckoning (The proposed method by the dissertation)
     * 
     * Inherits from the TraditionalDeadReckoning class for similar functionality save the proposed amendments to 
     * threshold and blend time. Included here is space afforded towards attempting to tend the prediction towards
     * the trend as described in Appendix 2 of the dissertation. This will be in GetProjectedPosition() overriden function
     */
    class LedReckoning : TraditionalDeadReckoning
    {
        // The minimum and maximum distance values for threshold based packet update requests
        public double MinimumThreshold, MaximumThreshold;

        // The current velocity trend for the simulations position
        private Vector velocityTrend;

        // The data table from which to look up the the current velocity trend
        private Vector[,] m_LedReckoningLevelDataTable;

        // The 0 to 1 correlation value of the current velocity to the trend velocity. 0 = perpendicular, 1 = parallel
        private double correlation;

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
        {
            get
            {
                // Get the current velocity trend
                velocityTrend = GetVelocityTrend(deadReckonedPosition.X, deadReckonedPosition.Y);
                
                // If there is a simulated velocity value to work with (be able to take a dot product from)
                if (simulatedVelocity.Length != 0.0 && velocityTrend.Length != 0.0f)
                {
                    // Update the correlation value
                    correlation = Math.Abs(Vector.Dot(simulatedVelocity.Normalised, velocityTrend.Normalised));
                }
                else
                {
                    // Default correlation is the uncommitted midpoint between 0 - 1.
                    correlation = 0.5f;
                }

                // Return the max at full correlation, min at zero, and a grade throughout for an intermediate value
                return MinimumThreshold + (MaximumThreshold - MinimumThreshold) * correlation;
            }
        }

        /*
         * Uses Newtonian motion equation to get a simulated position based off the last known position
         */
        protected override Vector GetProjectedPosition(Vector startingPosition, Vector velocity, float deltaTime)
        {
            // ... It is here that future work will attempt to tend the led reckoning simulation towards the velocity trend values
            // as discussed in the dissertation's Appendix 2. For now, we will mirror the dead reckoning approach

            // Second order derivitive prediction using Newtonian laws of motion
            return startingPosition + (velocity * deltaTime) + (lastKnownAcceleration * 0.5f * deltaTime * deltaTime);
        }

        /*
         * The time in seconds allowed for the projection using the simulation values to interpolate and
         * reconcile with the projection using the last known values.
         * 
         * Alters with the threshold as justified in the dissertation
         */
        protected override float blendTime
        {
            get
            {
                return 80.0f * (float)(1.0 / Threshold) / Experiment.framesPerSecond;
            }
        }

        /*
         * Takes level position values, and returns the velocity trend found for that position in the LedReckoning level data
         */
        private Vector GetVelocityTrend(double playerPositionX, double playerPositionY)
        {
            // Translate the in game position into appropriate data structure look up coordinates
            int[] lookUpCoords = InfernoLevelData.TranslatePositionIntoLookUpCoordinates(playerPositionX, playerPositionY);

            // Return the looked up Vector
            return m_LedReckoningLevelDataTable[lookUpCoords[0], lookUpCoords[1]];
        }
    }
}
