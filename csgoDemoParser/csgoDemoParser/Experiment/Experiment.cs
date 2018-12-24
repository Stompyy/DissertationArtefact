using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace csgoDemoParser
{
    class Experiment
    {
        public const int LevelAxisSubdivisions = 100;

        const double traditionalDRThreshold = 100.0;
        const double minimumAllowedThreshold = 10.0;
        const double maximumAllowedThreshold = 200.0;

        PlayerPathLoader m_PlayerPath;
        Vector[,] m_LedReckoningLevelDataTable;

        LedReckoning m_LedReckoning;
        TraditionalDeadReckoning m_TraditionalDeadReckoning;

        public Experiment(PlayerPathLoader playerPath, Vector[,] ledReckoningLevelDataTable)
        {
            m_PlayerPath = playerPath;
            m_LedReckoningLevelDataTable = ledReckoningLevelDataTable;

            //
            m_LedReckoning = new LedReckoning()
            {
                MinimumThreshold = minimumAllowedThreshold,
                MaximumThreshold = maximumAllowedThreshold
            };

            //
            m_TraditionalDeadReckoning = new TraditionalDeadReckoning()
            {
                Threshold = traditionalDRThreshold
            };
        }

        public void Update(Vector currentVelocity, Vector velocityTrend)
        {
            m_LedReckoning.Threshold(currentVelocity, velocityTrend);


        }

        private Vector getVelocityTrend(double playerPositionX, double playerPositionY)
        {
            int[] lookUpCoords = InfernoLevelData.translatePositionIntoLookUpCoordinates(playerPositionX, playerPositionY);

            return m_LedReckoningLevelDataTable[lookUpCoords[0], lookUpCoords[1]];
        }
    }
}
