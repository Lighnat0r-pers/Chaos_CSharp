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
        public BaseEffect(string _name)
        {
            name = _name;
        }

        /// <summary>
        /// Constructor for BaseEffect class.
        /// </summary>
        public BaseEffect(string _name, string _category, int _difficulty)
        {
            name = _name;
            category = _category;
            difficulty = _difficulty;
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
