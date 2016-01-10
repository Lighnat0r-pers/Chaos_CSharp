using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace ChaosMod
{
    class Modules
    {
        private List<IModuleHandler> ModuleHandlers { get; } = new List<IModuleHandler>();

        private int RefreshRate => 100; // Milliseconds

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

        private void InitTimedEffectsModule()
        {
            Debug.WriteLine("Initializing timed effects module.");
            TimedEffectHandler timedEffectHandler = new TimedEffectHandler(DataFileHandler.ReadTimedEffects(Settings.Game));
            ModuleHandlers.Add(timedEffectHandler);
        }

        private void InitPermanentEffectsModule()
        {
            Debug.WriteLine("Initializing permanent effects module.");
        }

        private void InitStaticEffectsModule()
        {
            Debug.WriteLine("Initializing static effects module.");
        }

        /// <summary>
        /// This method is in charge of activating the different modules in the mod (e.g. TimedEffects, StaticEffects etc)
        /// </summary>
        public void Update()
        {
            foreach (var moduleHandler in ModuleHandlers)
            {
                moduleHandler.Update();
            }
            Thread.Sleep(RefreshRate);
        }

        public void Shutdown()
        {
            foreach (var moduleHandler in ModuleHandlers)
            {
                moduleHandler.Shutdown();
            }
        }

    }
}
