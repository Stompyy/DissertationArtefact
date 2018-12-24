using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csgoDemoParser
{
    public class TraditionalDeadReckoning
    {
        /*
         * 
         */
        public double Threshold;

        /*
         * 
         */
        public static Vector Prediction(Vector lastKnownPosition, Vector lastKnownVelocity, Vector lastKnownAcceleration, float lastKnownTime, float predictionTime)
        {
            float deltaTime = predictionTime - lastKnownTime;

            return lastKnownPosition + (lastKnownVelocity * deltaTime /*convert to secs?*/) + (lastKnownAcceleration * 0.5f * deltaTime * deltaTime/*convert to secs?*/);
        }
    }
}
