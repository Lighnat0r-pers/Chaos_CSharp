using System;
using System.Collections.Generic;

namespace ChaosMod
{
    class TimedEffectHandler : IModuleHandler
    {
        private Random debugRandom;

        private List<TimedEffect> timedEffects;
        private TimedEffect currentEffect;
        private EffectTimer effectTimer;

        private bool ShouldAbort => Settings.Game.BaseChecks.Exists(c => !c.Succeeds() && c.onFail == BaseCheck.Abort);
        private bool ShouldSuspend => Settings.Game.BaseChecks.Exists(c => !c.Succeeds() && (c.onFail == BaseCheck.Suspend || c.onFail == BaseCheck.Abort));

        private TimedEffect CurrentEffect
        {
            get { return currentEffect; }

            set
            {
                currentEffect = value;
                Settings.CurrentEffect = value;
            }
        }

        public TimedEffectHandler(List<TimedEffect> timedEffects)
        {
            this.timedEffects = timedEffects;
            debugRandom = new Random(Settings.Seed);
            effectTimer = new EffectTimer();
        }

        public void Update()
        {
            // TODO(Ligh): Build in debug tools for manipulating the timed effects.

            // If the base limitations validate and no effect is active, get the next effect and activate it.
            if (CurrentEffect == null)
            {
                // TODO(Ligh): This needs to support gradual activations somehow.

                while (!ShouldSuspend && CurrentEffect == null)
                {
                    var effect = DebugGetNextEffect();

                    if (effect.CanActivate())
                    {
                        effect.Activate();
                        CurrentEffect = effect;
                        effectTimer.SetDuration(effect.effectLength);
                    }
                }
            }
            else
            {
                // Do stuff when there's already an active effect.

                if (ShouldAbort || effectTimer.EndTimeHasPassed())
                {
                    CurrentEffect.Deactivate();
                    CurrentEffect = null;
                }
                else if (ShouldSuspend)
                {
                    CurrentEffect.Deactivate();
                }
                else
                {
                    // NOTE(Ligh): This doesn't actually reactivate the effect if it is already activated.
                    CurrentEffect.Activate();
                }

                // TODO(Ligh): Add a mechanism that stops the game from resetting whatever the effect did (e.g. by continuously activating the effect).
            }
        }

        public void Shutdown()
        {
            CurrentEffect?.Deactivate();
            CurrentEffect = null;
        }

        public TimedEffect DebugGetNextEffect()
        {
            return timedEffects[debugRandom.Next(timedEffects.Count)];
        }
    }
}
