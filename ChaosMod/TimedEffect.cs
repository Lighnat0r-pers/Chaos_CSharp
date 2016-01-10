using System.Collections.Generic;

namespace ChaosMod
{
    class TimedEffect
    {
        static private long defaultEffectLength => 30000;

        public enum ActivationState
        {
            Inactive = 0,
            Active = 1,
            Suspended = 2,
        }

        private List<EffectActivator> activators;
        private List<Limitation> limitations;

        public bool IsInactive => Activation == ActivationState.Inactive;
        public bool IsActive => Activation == ActivationState.Active;
        public bool IsSuspended => Activation == ActivationState.Suspended;

        public string Name { get; private set; }
        public ActivationState Activation { get; private set; }

        public string Category { get; set; }
        public int Difficulty { get; set; }

        /// <summary>
        /// Duration of the effect taking time multipliers into account.
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

            foreach (var activator in activators)
            {
                activator.effect = this;
            }
        }

        public bool CanActivate()
        {
            return limitations.TrueForAll(l => l.Check());
        }

        public void Activate()
        {
            foreach (var activator in activators)
            {
                activator.Activate();
            }

            Activation = ActivationState.Active;
        }

        public void Deactivate()
        {
            foreach (var activator in activators)
            {
                activator.Deactivate();
            }

            Activation = ActivationState.Inactive;
        }

        public void Suspend()
        {
            Deactivate();
            Activation = ActivationState.Suspended;
        }
    }
}
