using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ChaosMod
{
    class TimedEffectHandler : IModuleHandler
    {
        private Random debugRandom;

        private List<TimedEffect> TimedEffects { get; }

        private EffectTimer effectTimer { get; } = new EffectTimer();

        private TimedEffect currentEffect;

        private bool ShouldAbort => Settings.Game.BaseChecks.Exists(c => !c.Succeeds() && c.FailType == FailType.Abort);
        private bool ShouldSuspend => Settings.Game.BaseChecks.Exists(c => !c.Succeeds() && (c.FailType == FailType.Suspend || c.FailType == FailType.Abort));

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
            TimedEffects = timedEffects;
            debugRandom = new Random(Settings.Seed);
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

                    if (effect.CanActivate)
                    {
                        Debug.WriteLine($"Activating timed effect: {effect.Name}");

                        effect.Activate();
                        CurrentEffect = effect;
                        effectTimer.SetDuration(effect.EffectLength);
                    }
                }
            }
            else
            {
                // Do stuff when there's already an active effect.

                if (ShouldAbort || effectTimer.EndTimeHasPassed())
                {
                    if (ShouldAbort)
                    {
                        Debug.WriteLine($"Aborting timed effect: {CurrentEffect.Name}");
                    }
                    else
                    {
                        Debug.WriteLine($"Deactivating timed effect: {CurrentEffect.Name}");
                    }

                    CurrentEffect.Deactivate();
                    CurrentEffect = null;
                }
                else if (ShouldSuspend)
                {
                    if (CurrentEffect.IsActive)
                    {
                        Debug.WriteLine($"Suspending timed effect: {CurrentEffect.Name}");
                    }

                    CurrentEffect.Suspend();
                }
                else
                {
                    if (!CurrentEffect.IsActive)
                    {
                        Debug.WriteLine($"Reactivating timed effect: {CurrentEffect.Name}");
                    }

                    CurrentEffect.Activate();
                }
            }
        }

        public void Shutdown()
        {
            if (CurrentEffect != null)
            {
                Debug.WriteLine($"Deactivating timed effect for shutdown: {CurrentEffect.Name}");

            }

            CurrentEffect?.Deactivate();
            CurrentEffect = null;
        }

        public TimedEffect DebugGetNextEffect()
        {
            return TimedEffects[debugRandom.Next(TimedEffects.Count)];
        }
    }
}
