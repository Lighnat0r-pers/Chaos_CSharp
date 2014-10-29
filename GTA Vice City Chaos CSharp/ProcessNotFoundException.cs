using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTAVC_Chaos
{
    class ProcessNotFoundException : Exception
    {
        public ProcessNotFoundException()
        {

        }

        public ProcessNotFoundException(string message)
            : base(message)
        {

        }

        public ProcessNotFoundException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
