using System.Collections.Generic;

namespace ChaosMod
{
    class LimitationCheck : ICheck
    {
        private Limitation Limitation { get; }

        public LimitationCheck(Limitation limitation, bool target, Dictionary<string, string> parameters)
        {
            limitation.Target = target;
            limitation.SetParameters(parameters);
            Limitation = limitation;
        }

        public bool Succeeds()
        {
            return Limitation.Check();
        }
    }
}
