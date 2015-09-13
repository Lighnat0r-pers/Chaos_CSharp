using System;

namespace GTAVC_Chaos
{
    class TimedEffectHandler : IModuleHandler
    {
        private Random debugRandom;

        public TimedEffect[] timedEffects;

        public bool effectActive = false;
        public TimedEffect currentEffect;
        public EffectTimer effectTimer;

        public TimedEffectHandler(TimedEffect[] timedEffects)
        {
            this.timedEffects = timedEffects;
            debugRandom = new Random(Settings.seed);
            effectTimer = new EffectTimer();
        }

        public void Update()
        {
            // TODO(Ligh): Build in debug tools for manipulating the timed effects.

            // TODO(Ligh): Check base limitations. If they fail, deactivate the active effect if there is one.


            // NOTE(Ligh): If the base limitations validate and no effect is active, get the next effect and activate it.
            // TODO(Ligh): Handle case where the effect chosen cannot be activated because of its limitations.
            if (!effectActive)
            {
                // TODO(Ligh): This needs to support gradual activations somehow.
                TimedEffect effect = DebugGetNextEffect();
                bool succeeded = effect.Activate();
                if (succeeded)
                {
                    effectActive = true;
                    currentEffect = effect;
                    effectTimer.SetDuration(effect.effectLength);
                }
            }
            else
            {
                // NOTE(Ligh): Do stuff when there's already an active effect.

                if (effectTimer.EndTimeHasPassed())
                {
                    currentEffect.Deactivate();
                    effectActive = false;
                    currentEffect = null;
                }

                // TODO(Ligh): Add a mechanism that stops the game from resetting whatever the effect did (e.g. by continuously activating the effect).
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
           // index = 3;
            return timedEffects[index];
        }
    }
}
