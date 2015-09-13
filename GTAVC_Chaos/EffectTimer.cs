using System;

namespace GTAVC_Chaos
{
    class EffectTimer
    {
        private bool usingIGT;
        private long startTime;
        private long endTime;

        private MemoryAddress GameTime;

        public EffectTimer()
        {
            GameTime = Program.game.FindMemoryAddressByName("GameTimeInMS");
            if (GameTime != null)
            {
                // IGT available, use IGT
                usingIGT = true;
            }
            else
            {
                // IGT not available: use realtime.
                usingIGT = false;
            }
        }

        // NOTE(Ligh): This function is not interested in millisecond resolution, just a 
        // somewhat accurate (<100ms is ideal, but anything up to 1000ms would work, really)
        // timestamp in milliseconds.
        private long GetCurrentTime()
        {
            long result;
            if (usingIGT)
            {
                result = (long)GameTime.Read();
            }
            else
            {
                result = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            }
            return result;
        }

        private void UpdateStartTime()
        {
            startTime = GetCurrentTime();
        }

        public void SetDuration(long duration)
        {
            UpdateStartTime();
            endTime = startTime + duration;
        }

        public bool EndTimeHasPassed()
        {
            bool result = false;

            long currentTime = GetCurrentTime();

            if (currentTime > endTime)
            {
                result = true;
            }

            // NOTE(Ligh): This should never occur but we check it just in case 
            // to avoid effects lasting longer than they should.
            if (currentTime < startTime)
            {
                result = true;
            }

            return result;
        }
    }
}
