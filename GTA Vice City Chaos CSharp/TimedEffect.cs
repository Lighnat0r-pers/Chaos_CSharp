using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAVC_Chaos
{
    class TimedEffect : BaseEffect
    {
        static int defaultEffectLength = 30000;


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
        public TimedEffect(string effectName, int length) : base(effectName)
        {
            effectLength = length;

        }
    }
}
