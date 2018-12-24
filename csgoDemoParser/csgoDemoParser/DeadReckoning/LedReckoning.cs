using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csgoDemoParser
{
    /*
     * Level ... Dead Reckoning
     * 
     * Educated
     * Experiential
     */
    class LedReckoning
    {
        /*
         * 
         */
        public double MinimumThreshold, MaximumThreshold;
        
        /*
         * 
         */
        public double Threshold(Vector currentVelocity, Vector velocityTrend)
        {
            // Should take magnitude into account also... areas of near zero overall velocity trend should not as be subject
            // Vector length is the velocity so just 
            //velocityTrend.Length / InfernoLevelData.maxPlayerSpeed

            // The confidence value is between -1 to 1 for direction vector
            double confidence = Vector.Dot(currentVelocity.Normalised, velocityTrend.Normalised);

            return MinimumThreshold + (MaximumThreshold - MinimumThreshold) * Math.Abs(confidence);
        }

        /*
         * 
         */
        public static Vector Prediction(Vector lastKnownPosition, Vector lastKnownVelocity, Vector lastKnownAcceleration, float lastKnownTime, float predictionTime, Vector velocityTrend, Vector accelerationTrend)
        {
            // Get the time passed since ...
            float deltaTime = predictionTime - lastKnownTime;

            // The confidence value is between -1 to 1 for direction vector
            double confidence = Vector.Dot(lastKnownVelocity.Normalised, velocityTrend.Normalised);

            Vector ledAcceleration = (lastKnownAcceleration + accelerationTrend * confidence) / 2.0f;

            return lastKnownPosition + (lastKnownVelocity * deltaTime /*convert to secs?*/) + (ledAcceleration * 0.5f * deltaTime * deltaTime/*convert to secs?*/);
        }
    }
}
