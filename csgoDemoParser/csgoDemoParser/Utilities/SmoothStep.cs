
namespace csgoDemoParser
{
    class Utilities
    {
        /*
         * Standard bell shaped lerping curve from 0 - 1
         * @return double
         */
        public static double DSmoothStep(double x)
        {
            return x * x * (3.0 - 2.0 * x);
        }

        /*
         * Standard bell shaped lerping curve from 0 - 1
         * @return float
         */
        public static float FSmoothStep(double x)
        {
            return (float)(x * x * (3.0 - 2.0 * x));
        }

        /*
         * Smoother bell shaped lerping curve from 0 - 1
         * @return double
         */
        public static double DSmootherStep(double x)
        {
            return x * x * x * (x * (x * 6.0 - 15.0) + 10.0);
        }

        /*
         * Smoother bell shaped lerping curve from 0 - 1
         * @return float
         */
        public static float FSmootherStep(double x)
        {
            return (float)(x * x * x * (x * (x * 6.0 - 15.0) + 10.0));
        }
    }
}
