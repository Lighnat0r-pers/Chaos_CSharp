
namespace GTAVC_Chaos
{
    class TimedEffect : BaseEffect
    {
        static private int defaultEffectLength = 30000;

        private EffectActivator[] activators;
        private Limitation[] limitations;

        private bool canExecute;

        /// <summary>
        /// Property effectLength which automatically takes the timeMultiplier as defined in Settings.cs into account.
        /// </summary>
        public int effectLength
        {
            get { return length; }
            set { length = value * Settings.timeMultiplier; }
        }
        private int length;

        /// <summary>
        /// Constructor for TimedEffect class specifying the name and duration.
        /// The name is passed onto the constructor of the base class (BaseEffect).
        /// </summary>
        public TimedEffect(string name, string category, int difficulty, EffectActivator[] activators, int duration = 0, Limitation[] limitations = null)
            : base(name, category, difficulty)
        {
            effectLength = (duration == 0) ? defaultEffectLength : duration;

            this.activators = activators;
            this.limitations = limitations;
        }

        // Activate the effect
        public override void Activate()
        {
            // Check all the limitations, if one of them returns true set canExecute to false and stop checking limitations.
            foreach (Limitation limitation in limitations)
            {
                if (!limitation.Check())
                {
                    canExecute = false;
                    break;
                }
                else
                {
                    canExecute = true;
                }
            }

            if (canExecute == true)
            {

            }
        }

        // Deactivate the effect
        public override void Deactivate()
        {

        }

        public void CalculateMTTH()
        {
            float activationTime = effectLength / difficulty;
        }
    }
}
