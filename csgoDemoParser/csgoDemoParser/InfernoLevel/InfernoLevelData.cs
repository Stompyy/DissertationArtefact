using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csgoDemoParser
{
    class InfernoLevelData
    {
        // Found by performing a min max query on master table, returns -1964, 2704, -791.9999, 3600
        // Rounded these numbers
        public const double minimumXValue = -1960.0;
        public const double maximumXValue = 2710.0;
        public const double minimumYValue = -790.0;
        public const double maximumYValue = 3605.0;

        private const double mapTotalX = maximumXValue - minimumXValue;
        private const double mapTotalY = maximumYValue - minimumYValue;

        // This is a constant for the game counter strike: global offensive. 
        // Confirmed by using console commands in game 
        public const float maxPlayerSpeed = 300.0f;

        // The size of each grid piece
        public const double subdivisionSizeX = mapTotalX / (double)Experiment.LevelAxisSubdivisions;
        public const double subdivisionSizeY = mapTotalY / (double)Experiment.LevelAxisSubdivisions;


        /*
         * Converts an in game position into the appropriate look up coordinates for the 
         * Experiment.LevelAxisSubdivisions sized data structure
         */
        public static int[] TranslatePositionIntoLookUpCoordinates(double x, double y)
        {
            // Sanity check that the value is a real position in the game
            x = Clamp(x, minimumXValue, maximumXValue-1);
            y = Clamp(y, minimumYValue, maximumYValue-1);

            // Calculate the return value
            int returnX = (int)((x - minimumXValue) / subdivisionSizeX);
            int returnY = (int)((y - minimumYValue) / subdivisionSizeY);

            return new int[] { returnX, returnY };
        }


        /*
         * 
         */
        public static VisualisationData TranslatePositionIntoRenderCoordinates(double posX, double posY, double imageWidth, double imageHeight)
        {
            // Sanity check that the value is a real position in the game
            posX = Clamp(posX, minimumXValue, maximumXValue - 1);
            posY = Clamp(posY, minimumYValue, maximumYValue - 1);
            
            // Calculate the return value
            float returnX = (float)(imageWidth * (posX - minimumXValue) / mapTotalX);
            float returnY = (float)(imageHeight - imageHeight * (posY - minimumYValue) / mapTotalY);    // Flip the up down to appear right

            return new VisualisationData(returnX, returnY);
        }

        /*
         * Clamps a value between min and max
         */
        private static double Clamp(double value, double min, double max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }
}
