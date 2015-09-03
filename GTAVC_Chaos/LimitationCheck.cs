using System;
using System.Collections.Generic;

namespace GTAVC_Chaos
{
    class LimitationCheck : ICheck
    {
        private string limitationName;
        private Dictionary<string, dynamic> parameters;

        public Limitation limitation;
        public bool target;

        public LimitationCheck(string limitationName, dynamic target, Dictionary<string, dynamic> parameters = null)
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
            limitation = Program.components.FindLimitationByName(limitationName);
            if (parameters != null)
            {
                limitation.setParameters(parameters);
            }
            limitation.setTarget(target);
        }
    }
}
