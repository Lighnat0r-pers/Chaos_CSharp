using System;
using System.Collections.Generic;

namespace ChaosMod
{
    class Limitation
    {
        private string name;
        private List<ICheck> checks;

        public bool Target { private get; set; }

        public Limitation(string name, List<ICheck> checks)
        {
            this.name = name;
            this.checks = checks;
        }

        public void SetParameters(Dictionary<string, string> parameters)
        {
            foreach (ParameterCheck check in checks.FindAll(c => c is ParameterCheck))
            {
                if (parameters?.ContainsKey(check.Address.Name) ?? false)
                {
                    check.Parameter = check.Address.ConvertToRightDataType(parameters[check.Address.Name]);
                }
                else if (check.Parameter == null)
                {
                    throw new ArgumentNullException(nameof(parameters), $"Missing parameter for parameter check {check.Address.Name} in {name} limitation");
                }
            }
        }

        /// <returns>True if all checks are passed, false otherwise.</returns>
        /// TODO(Ligh): This function name is vague on when it returns what.
        public bool Check()
        {
            return checks.TrueForAll(c => c.Succeeds() == Target);
        }
    }
}
