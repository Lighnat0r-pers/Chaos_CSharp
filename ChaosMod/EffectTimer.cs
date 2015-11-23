using System;

namespace ChaosMod
{
    class EffectTimer
    {
        private long startTime;
        private long endTime;

        private MemoryAddress GameTime;

        public EffectTimer()
        {
            // Try to find an IGT address to use. If there isn't one, realtime will be used.
            GameTime = Program.game.FindMemoryAddressByName("GameTimeInMS");
        }

        // NOTE(Ligh): This function does not offer millisecond resolution, just a 
        // somewhat accurate timestamp in milliseconds.
        private long GetCurrentTime()
        {
            return GameTime?.Read() ?? DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public void SetDuration(long duration)
        {
            startTime = GetCurrentTime();
            endTime = startTime + duration;
        }

        public bool EndTimeHasPassed()
        {
            // NOTE(Ligh): startTime should never be less than current time but we check it
            // just in case to avoid really long effects.
            return (GetCurrentTime() > endTime || GetCurrentTime() < startTime);
        }
    }
}
