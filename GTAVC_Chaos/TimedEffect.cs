using System.Collections.Generic;
using System.Diagnostics;

namespace GTAVC_Chaos
{
    class TimedEffect
    {
        static private long defaultEffectLength = 30000;

        private List<EffectActivator> activators;
        private List<Limitation> limitations;

        public string name;
        public string category;
        public int difficulty;

        /// <summary>
        /// Property effectLength which automatically takes the timeMultiplier into account.
        /// </summary>
        public long effectLength
        {
            get { return length; }
            set { length = value * Settings.timeMultiplier; }
        }
        private long length;

        public TimedEffect(string name, string category, int difficulty, List<EffectActivator> activators, uint duration = 0, List<Limitation> limitations = null)
        {
            effectLength = duration == 0 ? defaultEffectLength : duration;

            this.activators = activators;
            this.limitations = limitations;
            this.name = name;
            this.category = category;
            this.difficulty = difficulty;
        }

        public bool CanActivate()
        {
            return limitations.TrueForAll(l => l.Check());
        }

        /// <returns>True if successful, false otherwise.</returns>
        public void Activate()
        {
            foreach (var activator in activators)
            {
                activator.Activate();
            }

            Debug.WriteLine($"Activated timed effect: {name}");
        }

        public void Deactivate()
        {
            foreach (var activator in activators)
            {
                activator.Deactivate();
            }

            Debug.WriteLine($"Deactivated timed effect: {name}");
        }
    }
}
