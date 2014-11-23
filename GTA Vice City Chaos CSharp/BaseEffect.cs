using System;

namespace GTAVC_Chaos
{
    class BaseEffect
    {

        public bool enabled = true;
        public string name;
        public int difficulty;
        public string category;

        /// <summary>
        /// Constructor for BaseEffect class specifying only the name.
        /// </summary>
        public BaseEffect(string effectName)
        {
            name = effectName;
        }

        // Activate the effect
        public virtual void Activate()
        {

        }

        // Deactivate the effect
        public virtual void Deactivate()
        {

        }
    }
}
