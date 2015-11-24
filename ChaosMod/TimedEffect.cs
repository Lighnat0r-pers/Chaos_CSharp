using System.Collections.Generic;
using System.Diagnostics;

namespace ChaosMod
{
    class TimedEffect
    {
        static private long defaultEffectLength => 30000;

        private List<EffectActivator> activators;
        private List<Limitation> limitations;

        public string Name { get; private set; }
        public bool Activated { get; private set; }

        public string Category { get; set; }
        public int Difficulty { get; set; }

        /// <summary>
        /// Property effectLength which automatically takes the timeMultiplier into account.
        /// </summary>
        public long effectLength
        {
            get { return length; }
            set { length = value * Settings.TimeMultiplier; }
        }
        private long length;

        public TimedEffect(string name, string category, int difficulty, List<EffectActivator> activators, uint duration = 0, List<Limitation> limitations = null)
        {
            effectLength = duration == 0 ? defaultEffectLength : duration;

            this.activators = activators;
            this.limitations = limitations;
            Name = name;
            Category = category;
            Difficulty = difficulty;
        }

        public bool CanActivate()
        {
            return limitations.TrueForAll(l => l.Check());
        }

        public void Activate()
        {
            if (!Activated)
            {
                foreach (var activator in activators)
                {
                    activator.Activate();
                }

                Activated = true;

                Debug.WriteLine($"Activated timed effect: {Name}");
            }
        }

        public void Deactivate()
        {
            if (Activated)
            {
                foreach (var activator in activators)
                {
                    activator.Deactivate();
                }

                Activated = false;

                Debug.WriteLine($"Deactivated timed effect: {Name}");
            }
        }
    }
}
