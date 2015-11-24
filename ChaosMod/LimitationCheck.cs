using System.Collections.Generic;

namespace ChaosMod
{
    class LimitationCheck : ICheck
    {
        private Limitation limitation;

        public LimitationCheck(Limitation limitation, bool target, Dictionary<string, string> parameters)
        {
            limitation.Target = target;
            limitation.SetParameters(parameters);
            this.limitation = limitation;
        }

        public bool Succeeds()
        {
            return limitation.Check();
        }
    }
}
