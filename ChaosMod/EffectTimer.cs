using System;

namespace ChaosMod
{
    class EffectTimer
    {
        private long startTime;
        private long endTime;
        private MemoryAddress GameTime;

        private long CurrentTime => GameTime?.Read() ?? DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

        public EffectTimer()
        {
            // Try to find an IGT address to use. If there isn't one, realtime will be used.
            GameTime = Settings.Game.FindMemoryAddressByName("GameTimeInMS");
        }

        public void SetDuration(long duration)
        {
            startTime = CurrentTime;
            endTime = startTime + duration;
        }

        public bool EndTimeHasPassed()
        {
            // NOTE(Ligh): startTime should never be less than current time but we check it
            // just in case to avoid really long effects.
            return (CurrentTime > endTime || CurrentTime < startTime);
        }
    }
}
