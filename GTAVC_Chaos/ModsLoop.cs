using System.Diagnostics;
using System.Threading;

namespace GTAVC_Chaos
{
    static class ModsLoop
    {
        public static bool effectActive = false;
        public static TimedEffect currentEffect;

        /// <summary>
        /// Main method that should be called continuously. This method is in charge of
        /// activating the different modules in the mod (e.g. TimedEffects, StaticEffects etc)
        /// </summary>
        static public void Update()
        {
            do
            {
                DebugReadAddresses();
                //UpdateTimedEffects();
                Thread.Sleep(Settings.DEFAULT_WAIT_TIME * 2);

            } while (IsGameRunning() && !Program.shouldStop);

            //Deactivate everything here
        }

        static public bool IsGameRunning()
        {
            return true;
        }

        static public int CheckGameStatus()
        {

            return 0;
        }

        static public void UpdateTimedEffects()
        {
            if (!Settings.timedEffectsEnabled)
            {
                return;
            }

            if (!effectActive)
            {
                TimedEffect effect = Program.game.DebugGetTimedEffect();
                bool succeeded = effect.Activate();
                if (succeeded)
                {
                    effectActive = true;
                    currentEffect = effect;
                }
            }
        }

        static public void UpdateOutputWindow()
        {
            if (Program.outputWindow != null)
            {
               // Program.outputWindow.Refresh();
            }
        }

        static public void DebugReadAddresses()
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
