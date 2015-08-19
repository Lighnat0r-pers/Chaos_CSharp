using System;
using System.Reflection;

namespace GTAVC_Chaos
{
    class TimedEffect : BaseEffect
    {
        static int defaultEffectLength = 30000;

        // Define our delegate type. Generally limitations do not have any input arguments,
        // but in case they do there is an option to have an indeterminate number of strings
        // since the arguments will either be strings or be able to be parsed from strings.
        public delegate bool Limitations(params string[] strings);

        Limitations limitations = null;

        bool canExecute;

        /// <summary>
        /// Property effectLength which automatically takes the timeMultiplier as defined in Settings.cs into account.
        /// </summary>
        public int effectLength
        {
            get { return effectLength; }
            set { effectLength = value * Settings.timeMultiplier; }
        }
        
        /// <summary>
        /// Constructor for TimedEffect class specifying the name and duration.
        /// The name is passed onto the constructor of the base class (BaseEffect).
        /// </summary>
        public TimedEffect(string _name, string _category, int _difficulty, int duration = 0, string[] _limitations = null)
            : base(_name, _category, _difficulty)
        {
            effectLength = duration == 0 ? defaultEffectLength : duration;

            // Process the limitations string array and for each element convert it to a method in TimedEffectLimitations.cs
            // then add it to a multicast delegate which can be called when needed to check all limitations.
            // If no limitations are put in, generate an empty string array so the foreach loop is effectively skipped but doesn't return an exception.
            _limitations = (_limitations == null) ? new string[] { } : _limitations;
            foreach (string element in _limitations)
            {
                MethodInfo mi = typeof(TimedEffectLimitations).GetMethod(element);
                limitations += (Limitations)Delegate.CreateDelegate(typeof(Limitations), mi);
            }
        }

        // Activate the effect
        public override void Activate()
        {
            // Check all the limitations, if one of them returns true set canExecute to false and stop checking limitations.
            foreach (Limitations f in limitations.GetInvocationList())
            {
                if (f())
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
