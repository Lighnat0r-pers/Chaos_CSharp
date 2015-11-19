using System;
using System.Collections.Generic;

namespace GTAVC_Chaos
{
    class LimitationCheck : ICheck
    {
        private string limitationName;
        private Dictionary<string, string> parameters;

        public Limitation limitation;
        public bool target;

        public LimitationCheck(string limitationName, bool target, Dictionary<string, string> parameters = null)
        {
            this.limitationName = limitationName;
            this.target = target;
            this.parameters = parameters;
        }

        private LimitationCheck(LimitationCheck check)
        {
            this.limitationName = check.limitationName;
            this.target = check.target;
            this.ResolveLimitation();
        }

        public Object Clone()
        {
            return new LimitationCheck(this);
        }

        public bool Check()
        {
            return limitation.Check();
        }

        public void ResolveLimitation()
        {
            limitation = Program.game.FindLimitationByName(limitationName);

            if (limitation == null)
            {
                throw new ArgumentOutOfRangeException("limitation", "Limitation for limitation check is not defined.");
            }

            limitation.setParameters(parameters);
            limitation.Target = target;
        }
    }
}
