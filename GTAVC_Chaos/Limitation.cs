using System;
using System.Collections.Generic;

namespace GTAVC_Chaos
{
    class Limitation
    {

        public string name;
        public ICheck[] checks;
        public bool target;

        public Limitation(string name, ICheck[] checks)
        {
            this.name = name;
            this.checks = checks;
        }

        private Limitation(Limitation limitation)
        {
            this.name = limitation.name;
            this.checks = new ICheck[limitation.checks.Length];

            int count = 0;
            foreach (ICheck check in limitation.checks)
            {
                this.checks[count++] = (ICheck) check.Clone();
            }
        }

        public Limitation Clone()
        {
            return new Limitation(this);
        }

        public void setParameters(Dictionary<string, dynamic> parameters)
        {
            if (parameters != null)
            {
                foreach (ICheck checkVar in checks)
                {
                    if (checkVar is ParameterCheck)
                    {
                        ParameterCheck check = checkVar as ParameterCheck;
                        if (parameters.ContainsKey(check.address.name))
                        {
                            check.SetParameter(parameters[check.address.name]);
                            parameters.Remove(check.address.name);
                        }
                        else
                        {
                            // Checks if a default value was already set; if not, throw an exception since we're missing required data.
                            if (check.parameter == null)
                            {
                                throw new Exception("Missing parameter for parameter check " + check.address.name + " in " + name + " limitation");
                            }
                        }
                    }
                }

                // Checks there are any parameter values left. All used values are removed, so we're dealing with excess data here, which is probably not intended.
                if (parameters.Count != 0)
                {
                    parameters.ToString();
                    throw new Exception("Excess parameter(s) for limitation " + name + ". Parameters: " + String.Join("; ", parameters));
                }
            }
        }

        public void setTarget(bool target)
        {
            this.target = target;
        }

        public bool Check()
        {
            bool result = true;
            foreach (ICheck check in checks)
            {
                result = result && check.Check();
            }

            return (result == target);
        }
    }
}
