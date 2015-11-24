using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace ChaosMod
{
    class Modules
    {
        private List<IModuleHandler> moduleHandlers = new List<IModuleHandler>();

        public Modules()
        {
            if (Settings.TimedEffectsEnabled)
            {
                InitTimedEffectsModule();
            }
            if (Settings.PermanentEffectsEnabled)
            {
                InitPermanentEffectsModule();
            }
            if (Settings.StaticEffectsEnabled)
            {
                InitStaticEffectsModule();
            }
        }

        public void InitTimedEffectsModule()
        {
            Debug.WriteLine("Initializing timed effects module.");
            TimedEffectHandler timedEffectHandler = new TimedEffectHandler(DataFileHandler.ReadTimedEffects(Settings.Game));
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
            Thread.Sleep(Settings.DefaultWaitTime * 2);
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
            foreach (MemoryAddress address in Settings.Game.memoryAddresses)
            {
                Debug.WriteLine($"Address: {address.name}, Value: {address.Read() as object}");
            }
        }
    }
}
