
namespace csgoDemoParser
{
    class Utilities
    {
        /*
         * Standard bell shaped lerping curve from 0 - 1
         */
        public static float SmoothStep(float x)
        {
            return x * x * (3.0f - 2.0f * x);
        }

        /*
         * Smoother bell shaped lerping curve from 0 - 1
         */
        public static float SmootherStep(float x)
        {
            return x * x * x * (x * (x * 6.0f - 15.0f) + 10.0f);
        }
    }
}
