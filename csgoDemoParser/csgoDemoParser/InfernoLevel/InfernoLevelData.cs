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
        public const double minimumXValue = -1960.0;
        public const double maximumXValue = 2710.0;
        public const double minimumYValue = -790.0;
        public const double maximumYValue = 3605.0;

        public const float maxPlayerSpeed = 300.0f;

        public const double deltaX = (maximumXValue - minimumXValue) / (double)Experiment.LevelAxisSubdivisions;
        public const double deltaY = (maximumYValue - minimumYValue) / (double)Experiment.LevelAxisSubdivisions;


        public static int[] translatePositionIntoLookUpCoordinates(double x, double y)
        {
            int returnX = (int)((x - minimumXValue) / deltaX);
            int returnY = (int)((y - minimumYValue) / deltaY);

            return new int[] { returnX, returnY };
        }
    }
}
