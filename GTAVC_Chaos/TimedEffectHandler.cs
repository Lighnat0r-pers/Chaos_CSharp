using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAVC_Chaos
{
    class TimedEffectHandler
    {
        private Random debugRandom;

        public TimedEffect[] timedEffects;

        public TimedEffectHandler(TimedEffect[] timedEffects)
        {
            this.timedEffects = timedEffects;
        }

        public void InitEffectPicker()
        {
            debugRandom = new Random(Settings.seed);
        }

        public TimedEffect DebugGetNextEffect()
        {
            int index = debugRandom.Next(timedEffects.Length);
            index = 3;
            return timedEffects[index];
        }
    }
}
