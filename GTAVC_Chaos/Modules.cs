using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace GTAVC_Chaos
{
    class Modules
    {
        private List<IModuleHandler> moduleHandlers = new List<IModuleHandler>();

        public void InitTimedEffectsModule()
        {
            Debug.WriteLine("Initializing timed effects module.");
            TimedEffectHandler timedEffectHandler = new TimedEffectHandler(DataFileHandler.InitTimedEffectsFromFile(Program.game));
            timedEffectHandler.InitEffectPicker();
            moduleHandlers.Add(timedEffectHandler);
        }

        public void InitPermanentEffectsModule()
        {
            Debug.WriteLine("Initializing permanent effects module.");
        }

        public void InitStaticEffectsModule()
        {
            Debug.WriteLine("Initializing static effects module.");
        }

        /// <summary>
        /// This method is in charge of activating the different modules in the mod (e.g. TimedEffects, StaticEffects etc)
        /// </summary>
        public void Update()
        {
            Program.game.GetHandle();

            while (GameIsRunning() && !Program.shouldStop)
            {
                DebugReadAddresses();
                foreach (IModuleHandler moduleHandler in moduleHandlers)
                {
                    moduleHandler.Update();
                }
                Thread.Sleep(Settings.DEFAULT_WAIT_TIME * 2);

            }

            // Deactivate everything here
            if (GameIsRunning())
            {
                foreach (IModuleHandler moduleHandler in moduleHandlers)
                {
                    moduleHandler.Shutdown();
                }
            }
        }

        public bool GameIsRunning()
        {
            return true;
        }

        public int CheckGameStatus()
        {

            return 0;
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
