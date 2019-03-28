using System;
using System.Diagnostics;

namespace csgoDemoParser
{
    class Timer
    {
        Stopwatch m_Stopwatch;

        public Timer()
        {
            // https://www.codeproject.com/Articles/61964/Performance-Tests-Precise-Run-Time-Measurements-wi
            m_Stopwatch = new Stopwatch();
        }

        public void Start()
        {
            m_Stopwatch.Reset();
            m_Stopwatch.Start();
        }

        public void Stop()
        {
            m_Stopwatch.Stop();
        }

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
