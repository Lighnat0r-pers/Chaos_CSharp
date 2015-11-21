using System;

namespace GTAVC_Chaos
{
    class LimitationCheck : ICheck
    {
        public Limitation limitation;

        public LimitationCheck(Limitation limitation)
        {
            this.limitation = limitation;
        }

        public bool Check()
        {
            return limitation.Check();
        }
    }
}
