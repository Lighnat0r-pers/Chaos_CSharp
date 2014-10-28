using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAVC_Chaos
{
    class BaseEffect
    {

        public bool enabled = true;
        public string name;



        /// <summary>
        /// Constructor for BaseEffect class specifying only the name.
        /// </summary>
        public BaseEffect(string effectName)
        {
            name = effectName;
        }


    }
}
