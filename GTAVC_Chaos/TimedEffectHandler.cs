using System;

namespace GTAVC_Chaos
{
    class TimedEffectHandler : IModuleHandler
    {
        private Random debugRandom;

        public TimedEffect[] timedEffects;

        public bool effectActive = false;
        public TimedEffect currentEffect;

        public TimedEffectHandler(TimedEffect[] timedEffects)
        {
            this.timedEffects = timedEffects;
        }

        public void InitEffectPicker()
        {
            debugRandom = new Random(Settings.seed);
        }

        public void Update()
        {
            if (!effectActive)
            {
                TimedEffect effect = DebugGetNextEffect();
                bool succeeded = effect.Activate();
                if (succeeded)
                {
                    effectActive = true;
                    currentEffect = effect;
                }
            }
        }

        public void Shutdown()
        {
            if (currentEffect != null)
            {
                currentEffect.Deactivate();
            }
        }

        public TimedEffect DebugGetNextEffect()
        {
            int index = debugRandom.Next(timedEffects.Length);
            index = 3;
            return timedEffects[index];
        }
    }
}
