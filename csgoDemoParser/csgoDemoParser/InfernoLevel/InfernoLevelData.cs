using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csgoDemoParser
{
    /*
     * Contains information specific to the chosen test bed level
     */
    class InfernoLevelData
    {
        // Found by performing a min max query on master table, returns -1964, 2704, -791.9999, 3600
        // For ease, I have rounded these numbers
        public const double minimumXValue = -1960.0;
        public const double maximumXValue = 2710.0;
        public const double minimumYValue = -790.0;
        public const double maximumYValue = 3605.0;

        // The total width and height of the map
        private const double mapTotalX = maximumXValue - minimumXValue;
        private const double mapTotalY = maximumYValue - minimumYValue;

        // This is a constant for the game Counter Strike: Global Offensive. 
        // Confirmed by using console commands in game 
        public const float maxPlayerSpeed = 250.0f;

        // The size of each grid piece
        public const double subdivisionSizeX = mapTotalX / (double)Experiment.LevelAxisSubdivisions;
        public const double subdivisionSizeY = mapTotalY / (double)Experiment.LevelAxisSubdivisions;


        /*
         * Converts an in game position into the appropriate look up coordinates for the 
         * Experiment.LevelAxisSubdivisions sized data structure
         * returning an int array allows the easy migration to a three dimensional data structure if required for future work
         */
        public static int[] TranslatePositionIntoLookUpCoordinates(double x, double y)
        {
            // Sanity check that the value is a real position in the game
            x = Clamp(x, minimumXValue, maximumXValue-1);
            y = Clamp(y, minimumYValue, maximumYValue-1);

            // Calculate the return value
            int returnX = (int)((x - minimumXValue) / subdivisionSizeX);
            int returnY = (int)((y - minimumYValue) / subdivisionSizeY);

            // Return the look up coordinates for the data structure for that in game position
            return new int[] { returnX, returnY };
        }


        /*
         * Translates an in game position to a render coordinate given the image dimensions
         */
        public static VisualisationData TranslatePositionIntoRenderCoordinates(double posX, double posY, double imageWidth, double imageHeight)
        {
            // Sanity check that the value is a real position in the game
            posX = Clamp(posX, minimumXValue, maximumXValue - 1);
            posY = Clamp(posY, minimumYValue, maximumYValue - 1);

            // Calculate the return value
            // Annoyingly, the level coordinates and the draw method will cause the image to be drawn upside down
            // compared to the convention that Inferno is typically displayed. So we flip the up down to appear right
            float returnX = (float)(imageWidth * (posX - minimumXValue) / mapTotalX);
            float returnY = (float)(imageHeight - imageHeight * (posY - minimumYValue) / mapTotalY);

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
