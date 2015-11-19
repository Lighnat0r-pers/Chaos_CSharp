using System;
using System.Collections.Generic;

namespace GTAVC_Chaos
{
    class Limitation
    {
        private bool target;

        public string name;
        public List<ICheck> checks;

        public bool Target
        {
            set { target = value; }
        }

        public Limitation(string name, List<ICheck> checks)
        {
            this.name = name;
            this.checks = checks;
        }

        private Limitation(Limitation limitation)
        {
            this.name = limitation.name;
            this.checks = new List<ICheck>();

            foreach (var check in limitation.checks)
            {
                this.checks.Add((ICheck)check.Clone());
            }
        }

        public Limitation Clone()
        {
            return new Limitation(this);
        }

        public void setParameters(Dictionary<string, string> parameters)
        {
            if (parameters != null)
            {
                foreach (ParameterCheck check in checks.FindAll(c => c is ParameterCheck))
                {
                    if (parameters.ContainsKey(check.address.name))
                    {
                        dynamic parameter = check.address.ConvertToRightDataType(parameters[check.address.name]);
                        check.SetParameter(parameter);
                        parameters.Remove(check.address.name);
                    }
                    else
                    {
                        // Checks if a default value was already set; if not, throw an exception since we're missing required data.
                        if (check.parameter == null)
                        {
                            throw new ArgumentNullException("parameter", String.Format("Missing parameter for parameter check {0} in {1} limitation", check.address.name, name));
                        }
                    }
                }

                // Checks there are any parameter values left. All used values are removed, so we're dealing with excess data here, which is probably not intended.
                // Note that this is not actually a fatal error, the program could continue just fine.
                if (parameters.Count != 0)
                {
                    throw new ArgumentOutOfRangeException("parameters", String.Format("Excess parameter(s) for limitation {0}. Parameters: {1}", name , String.Join("; ", parameters)));
                }
            }
        }

        /// <returns>True if all checks are passed, false otherwise.</returns>
        /// TODO(Ligh): This function name is vague on when it returns what.
        public bool Check()
        {
            return checks.TrueForAll(c => c.Check() == target);
        }
    }
}
