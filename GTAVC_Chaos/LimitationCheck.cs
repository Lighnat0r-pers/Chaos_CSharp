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

        public LimitationCheck(string _limitationName, dynamic _target, Dictionary<string, dynamic> _parameters = null)
        {
            limitationName = _limitationName;
            target = _target;
            parameters = _parameters;
        }

        private LimitationCheck(LimitationCheck check)
        {
            limitationName = check.limitationName;
            target = check.target;
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
