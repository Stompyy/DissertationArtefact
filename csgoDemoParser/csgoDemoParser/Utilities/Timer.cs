using System;
using System.Diagnostics;

namespace csgoDemoParser
{
    /*
     * Used for benchmarking and optimisation purposes
     */
    class Timer
    {
        // The class sits upon .NET frameworks Stopwatch class
        Stopwatch m_Stopwatch;
        
        /*
         * Constructor
         */
        public Timer()
        {
            m_Stopwatch = new Stopwatch();
        }
        
        /*
         * Resets the Stopwatch instance and starts it.
         */
        public void Start()
        {
            m_Stopwatch.Reset();
            m_Stopwatch.Start();
        }

        /*
         * stops the Stopwatch instance
         */
        public void Stop()
        {
            m_Stopwatch.Stop();
        }

        /*
         * returns a formatted string representation of the Stopwatch instance's elapsed time for feedback messages
         */
        public String GetTimeElapsed()
        {
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = m_Stopwatch.Elapsed;

            // Format and display the TimeSpan value.
            return String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
        }
    }
}
