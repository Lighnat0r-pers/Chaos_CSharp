using System;

namespace GTAVC_Chaos
{
    class BaseEffect
    {

        public bool enabled = true;
        public string name;
        public string category;
        public int difficulty;

        /// <summary>
        /// Constructor for BaseEffect class specifying only the name.
        /// </summary>
        public BaseEffect(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Constructor for BaseEffect class.
        /// </summary>
        public BaseEffect(string name, string category, int difficulty)
        {
            this.name = name;
            this.category = category;
            this.difficulty = difficulty;
        }

        // Activate the effect
        public virtual bool Activate()
        {
            return false;
        }

        // Deactivate the effect
        public virtual void Deactivate()
        {

        }
    }
}
