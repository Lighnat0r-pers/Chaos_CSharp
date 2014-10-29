using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        Limitations allLimitations = null;

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
        /// Constructor for TimedEffect class specifying only the name.
        /// </summary>
        public TimedEffect(string effectName) : base(effectName)
        {
            effectLength = defaultEffectLength;
        }
        
        /// <summary>
        /// Constructor for TimedEffect class specifying the name and length.
        /// The name is passed onto the constructor of the base class (BaseEffect).
        /// </summary>
        public TimedEffect(string effectName, int duration, string[] limitations = null) : base(effectName)
        {
            //effectLength = duration;

            // Process the limitations string array and for each element convert it to a method in TimedEffectLimitations.cs
            // then add it to a multicast delegate which can be called when needed to check all limitations.
            // If no limitations are put in, generate an empty string array so the foreach loop is effectively skipped but doesn't return an exception.
            limitations = (limitations == null) ? new string[] { } : limitations;
            foreach (string element in limitations)
            {
                MethodInfo mi = typeof(TimedEffectLimitations).GetMethod(element);
                allLimitations += (Limitations)Delegate.CreateDelegate(typeof(Limitations), mi);
            }
        }


        // Activate the effect
        public void Activate()
        {
            // Check all the limitations, if one of them returns true set canExecute to false and stop checking limitations.
            foreach (Limitations f in allLimitations.GetInvocationList())
            {
                if (f())
                {
                    canExecute = false;
                    break;
                }
                else
                    canExecute = true;
            }
        }

        // Deactivate the effect
        public void Deactivate()
        {

        }
    }
}
