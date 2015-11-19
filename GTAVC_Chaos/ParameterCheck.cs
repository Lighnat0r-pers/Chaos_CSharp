using System;

namespace GTAVC_Chaos
{
    class ParameterCheck : ICheck
    {
        private string addressName;
        public MemoryAddress address;
        public dynamic parameter;

        public ParameterCheck(string addressName, dynamic parameter = null)
        {
            this.addressName = addressName;
            this.parameter = parameter;
        }

        private ParameterCheck(ParameterCheck check)
        {
            this.addressName = check.addressName;
            this.parameter = check.parameter;
            this.ResolveReferences();
        }

        public Object Clone()
        {
            return new ParameterCheck(this);
        }

        public void SetParameter(dynamic parameter)
        {
            this.parameter = parameter;
        }

        public bool Check()
        {
            return address.Read() == parameter;
        }

        public void ResolveReferences()
        {
            address = Program.game.FindMemoryAddressByName(addressName);

            if (address == null)
            {
                throw new ArgumentOutOfRangeException("address", "Memory address for check is not defined.");
            }

            if (parameter != null)
            {
                parameter = address.ConvertToRightDataType(parameter);
            }
        }
    }
}
