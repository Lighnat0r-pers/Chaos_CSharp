using System.Collections.Generic;

namespace ChaosMod
{
    public enum ActivationState
    {
        Inactive = 0,
        Active = 1,
        Suspended = 2,
    }

    class TimedEffect
    {
        static private long defaultEffectLength => 30000;

        /// <summary>
        /// Duration of the effect taking time multipliers into account.
        /// </summary>
        public long EffectLength
        {
            get { return effectLength; }
            set { effectLength = value * Settings.TimeMultiplier; }
        }
        private long effectLength;

        public string Category { get; set; }
        public int Difficulty { get; set; }

        public ActivationState Activation { get; private set; }

        public string Name { get; }

        public bool CanActivate => Limitations.TrueForAll(l => l.Check());
        public bool IsInactive => Activation == ActivationState.Inactive;
        public bool IsActive => Activation == ActivationState.Active;
        public bool IsSuspended => Activation == ActivationState.Suspended;

        private List<EffectActivator> Activators { get; }
        private List<Limitation> Limitations { get; }

        public TimedEffect(string name, string category, int difficulty, List<EffectActivator> activators, uint duration = 0, List<Limitation> limitations = null)
        {
            EffectLength = duration == 0 ? defaultEffectLength : duration;

            Activators = activators;
            Limitations = limitations;
            Name = name;
            Category = category;
            Difficulty = difficulty;

            foreach (var activator in activators)
            {
                activator.effect = this;
            }
        }

        public void Activate()
        {
            foreach (var activator in Activators)
            {
                activator.Activate();
            }

            Activation = ActivationState.Active;
        }

        public void Deactivate()
        {
            foreach (var activator in Activators)
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
