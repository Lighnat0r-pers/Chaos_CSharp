
namespace GTAVC_Chaos
{
    class TimedEffect : BaseEffect
    {
        static int defaultEffectLength = 30000;

        Limitation[] limitations;

        bool canExecute;

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
        public TimedEffect(string _name, string _category, int _difficulty, int duration = 0, Limitation[] _limitations = null)
            : base(_name, _category, _difficulty)
        {
            effectLength = (duration == 0) ? defaultEffectLength : duration;

            // Process the limitations string array and for each element convert it to a method in TimedEffectLimitations.cs
            // then add it to a multicast delegate which can be called when needed to check all limitations.
            // If no limitations are put in, generate an empty string array so the foreach loop is effectively skipped but doesn't return an exception.
            limitations = _limitations;
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
