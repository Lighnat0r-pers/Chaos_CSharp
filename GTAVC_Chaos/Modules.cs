using System.Diagnostics;
using System.Threading;

namespace GTAVC_Chaos
{
    class Modules
    {
        private TimedEffectHandler timedEffectHandler;

        public bool effectActive = false;
        public TimedEffect currentEffect;

        public void InitTimedEffectsModule(TimedEffect[] timedEffects)
        {
            timedEffectHandler = new TimedEffectHandler(timedEffects);
        }

        /// <summary>
        /// This method is in charge of activating the different modules in the mod (e.g. TimedEffects, StaticEffects etc)
        /// </summary>
        public void Update()
        {
            timedEffectHandler.InitEffectPicker();

            do
            {
                DebugReadAddresses();
                //UpdateTimedEffects();
                Thread.Sleep(Settings.DEFAULT_WAIT_TIME * 2);

            } while (IsGameRunning() && !Program.shouldStop);

            //Deactivate everything here
            if (IsGameRunning())
            {
                if (currentEffect != null)
                {
                    currentEffect.Deactivate();
                }

            }
        }

        public bool IsGameRunning()
        {
            return true;
        }

        public int CheckGameStatus()
        {

            return 0;
        }

        public void UpdateTimedEffects()
        {
            if (!Settings.timedEffectsEnabled)
            {
                return;
            }

            if (!effectActive)
            {
                TimedEffect effect = timedEffectHandler.DebugGetNextEffect();
                bool succeeded = effect.Activate();
                if (succeeded)
                {
                    effectActive = true;
                    currentEffect = effect;
                }
            }
        }

        public void UpdateOutputWindow()
        {
            if (Program.outputWindow != null)
            {

            }
        }

        public void DebugReadAddresses()
        {
            foreach (MemoryAddress address in Program.game.memoryAddresses)
            {
                dynamic value = address.Read();
                object a = value as object;
                Debug.WriteLine("Address " + address.name + ", value: " + a);
            }
        }
    }
}
