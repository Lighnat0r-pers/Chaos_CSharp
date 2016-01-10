using System;
using System.Collections.Generic;

namespace ChaosMod
{
    class Limitation
    {
        public bool Target { private get; set; }

        private string Name { get; }
        private List<ICheck> Checks { get; }

        public Limitation(string name, List<ICheck> checks)
        {
            Name = name;
            Checks = checks;
        }

        public void SetParameters(Dictionary<string, string> parameters)
        {
            foreach (ParameterCheck check in Checks.FindAll(c => c is ParameterCheck))
            {
                if (parameters?.ContainsKey(check.Address.Name) ?? false)
                {
                    check.Parameter = check.Address.ConvertToRightDataType(parameters[check.Address.Name]);
                }
                else if (check.Parameter == null)
                {
                    throw new ArgumentNullException(nameof(parameters), $"Missing parameter for parameter check {check.Address.Name} in {Name} limitation");
                }
            }
        }

        /// <returns>True if all checks are passed, false otherwise.</returns>
        /// TODO(Ligh): This function name is vague on when it returns what.
        public bool Check()
        {
            return Checks.TrueForAll(c => c.Succeeds() == Target);
        }
    }
}
