using System;

namespace ChaosMod
{
    class ParameterCheck : ICheck
    {
        public MemoryAddress address;
        public dynamic parameter;

        public ParameterCheck(MemoryAddress address, string parameter = null)
        {
            this.address = address;

            if (parameter != null)
            {
                this.parameter = address.ConvertToRightDataType(parameter);
            }
        }

        public void SetParameter(dynamic parameter)
        {
            this.parameter = parameter;
        }

        public bool Check()
        {
            return address.Read() == parameter;
        }
    }
}
