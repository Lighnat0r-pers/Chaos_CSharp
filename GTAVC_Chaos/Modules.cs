using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace GTAVC_Chaos
{
    class Modules
    {
        private List<IModuleHandler> moduleHandlers = new List<IModuleHandler>();

        public Modules()
        {
            if (Settings.timedEffectsEnabled)
            {
                InitTimedEffectsModule();
            }
            if (Settings.permanentEffectsEnabled)
            {
                InitPermanentEffectsModule();
            }
            if (Settings.staticEffectsEnabled)
            {
                InitStaticEffectsModule();
            }
        }

        public void InitTimedEffectsModule()
        {
            Debug.WriteLine("Initializing timed effects module.");
            TimedEffectHandler timedEffectHandler = new TimedEffectHandler(DataFileHandler.InitTimedEffectsFromFile(Program.game));
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
            //DebugReadAddresses();
            foreach (IModuleHandler moduleHandler in moduleHandlers)
            {
                moduleHandler.Update();
            }
            Thread.Sleep(Settings.DEFAULT_WAIT_TIME * 2);
        }

        public void Shutdown()
        {
            // Deactivate all modules here
            foreach (IModuleHandler moduleHandler in moduleHandlers)
            {
                moduleHandler.Shutdown();
            }
        }

        public void DebugReadAddresses()
        {
            foreach (MemoryAddress address in Program.game.memoryAddresses)
            {
                Debug.WriteLine("Address: {0}, Value: {1}", address.name, address.Read() as object);
            }
        }
    }
}
