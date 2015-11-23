using System;

namespace ChaosMod
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
