using System;

namespace GTAVC_Chaos
{
    class ParameterCheck : ICheck
    {
        public MemoryAddress address;
        public dynamic parameter;

        public ParameterCheck(MemoryAddress address, string parameter = null)
        {
            this.address = address;
            this.parameter = address.ConvertToRightDataType(parameter);
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
