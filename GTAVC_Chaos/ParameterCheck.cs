using System;

namespace GTAVC_Chaos
{
    class ParameterCheck : ICheck
    {
        public MemoryAddress address;
        public dynamic parameter;

        public ParameterCheck(MemoryAddress address, dynamic parameter = null)
        {
            this.address = address;
            this.parameter = parameter;
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
