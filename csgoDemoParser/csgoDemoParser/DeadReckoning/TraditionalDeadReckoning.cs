
namespace csgoDemoParser
{
    /*
     * Traditional threshold based dead reckoning client side prediction and simulation method
     * 
     * Also acts the base class for the dissertation's proposed LedReckoning class
     */
    public class TraditionalDeadReckoning
    {
        // The maximum distance in game units of the simulated position from the actual position allowed, before a simulated packet update is prompted
        public double Threshold;

        // Needed Vectors for the simulation
        public Vector

            // The projection's motion values
            deadReckonedPosition, simulatedVelocity, simulatedAcceleration,

            // Last known values from packet update
            lastKnownPosition, lastKnownVelocity, lastKnownAcceleration,

            // Starting values for the projection as per last update
            projectedPositionAtLastUpdate, projectedVelocityAtLastUpdate;

        // The frame number of the last simulated packet update for which Vectors lastKnown values apply
        public int lastKnownTime;

        // Used to track the number of prompted packet updates required
        public int numberOfUpdatesNeeded;

        // Used to adjust the DEM file velocity value to a per frame velocity value
        private float velocityMultiplier;

        /*
         * Constructor initialises values to the initialPosition or a zero value
         */
        public TraditionalDeadReckoning(Vector initialPosition)
        {
            // Initialise all position values to the initial position
            lastKnownPosition = initialPosition;
            deadReckonedPosition = initialPosition;
            projectedPositionAtLastUpdate = initialPosition;
            deadReckonedPosition = initialPosition;

            // Initialise all velocity and acceleration values to zero
            lastKnownVelocity = new Vector(0.0f);
            simulatedVelocity = new Vector(0.0f);
            lastKnownAcceleration = new Vector(0.0f);
            simulatedAcceleration = new Vector(0.0f);
            projectedVelocityAtLastUpdate = new Vector(0.0f);

            // Initialise the time to zero
            lastKnownTime = 0;

            // Initialise the measure of packet updates needed to zero
            numberOfUpdatesNeeded = 0;

            // Used to translate in game velocity values to per frame relevant value
            velocityMultiplier = InfernoLevelData.maxPlayerSpeed / Experiment.framesPerSecond;
        }

        /*
         * The time in seconds allowed for the projection using the simulation values to interpolate and
         * reconcile with the projection using the last known values.
         * 
         * protected virtual allows protected override in derived classes for different functionality
         */
        protected virtual float blendTime
        {
            get
            {
                return 4.0f / Experiment.framesPerSecond;
            }
        }

        /*
         * Uses Newtonian motion equation to get a simulated position based off the last known position
         * 
         * protected virtual allows protected override in derived classes for different functionality
         */
        protected virtual Vector GetProjectedPosition(Vector startingPosition, Vector velocity, float deltaTime)
        {
            // Second order derivitive prediction using Newtonian laws of motion
            return startingPosition + (velocity * deltaTime) + (lastKnownAcceleration * 0.5f * deltaTime * deltaTime);
        }

        /*
         * Dead reckoning implementation uing projective velocity blending
         */
        public Vector GetDeadReckonedPositionWithVelocityBlending(int predictionTime)
        {
            // Set up the delta time value since the last update, and adjusting for frames per second
            float deltaTime = (float)(predictionTime - lastKnownTime);
            deltaTime /= Experiment.framesPerSecond;

            // Run Newton algorithm on last known real values to give a vector position for our simulation to interpolate towards
            Vector projectionUsingLastKnownValues = GetProjectedPosition(
                lastKnownPosition,
                lastKnownVelocity * velocityMultiplier,
                deltaTime
                );

            // Do Velocity blending between current game simulation velocity and last known velocity
            simulatedVelocity = VectorBlend(projectedVelocityAtLastUpdate, lastKnownVelocity, deltaTime);

            // Run Newton algorithm using blended velocity upon the projection data
            Vector projectionUsingSimulationValues = GetProjectedPosition(
                projectedPositionAtLastUpdate,
                simulatedVelocity * velocityMultiplier,
                deltaTime
                );

            // Interpolate using Vector blend between simulation projection towards the projection using the last known values
            deadReckonedPosition = VectorBlend(projectionUsingSimulationValues, projectionUsingLastKnownValues, deltaTime);

            return deadReckonedPosition;
        }

        /*
         * Blends the startVector towards the targetVector as deltaTime tends towards blendTime
         */
        private Vector VectorBlend(Vector startVector, Vector targetVector, float deltaTime)
        {
            // Clamp between zero to one to avoid overshooting the target blend amount
            float blendWeight =  ClampZeroToOne(deltaTime / blendTime);

            // Return the blended Vector between startVector and targetVector
            return startVector + (targetVector - startVector) * blendWeight;
        }

        /*
         * Clamps a value between zero and one
         */
        private float ClampZeroToOne(float value)
        {
            if (value < 0.0f) return 0.0f;
            if (value > 1.0f) return 1.0f;
            return value;
        }

        /*
         * Simulates a network packet update, updates the last known values to the current values
         */
        public void SimulatePacketUpdate(Vector currentPosition, Vector currentVelocity, Vector currentAcceleration, int currentTime)
        {
            // Update the values
            lastKnownPosition = currentPosition;
            lastKnownVelocity = currentVelocity;
            lastKnownAcceleration = currentAcceleration;
            lastKnownTime = currentTime;

            // Update the projection start values to the current values
            projectedPositionAtLastUpdate = deadReckonedPosition;
            projectedVelocityAtLastUpdate = simulatedVelocity;

            // Also increments the number of updates that this simulation has used as a measure of simulation accuracy
            numberOfUpdatesNeeded++;
        }
    }
}
