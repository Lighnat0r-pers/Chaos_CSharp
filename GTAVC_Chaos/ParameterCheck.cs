using System;

namespace GTAVC_Chaos
{
    class ParameterCheck : ICheck
    {
        public MemoryAddress address;
        public dynamic parameter;

        public ParameterCheck(MemoryAddress _address, dynamic _parameter = null)
        {
            address = _address;
            if (_parameter != null)
            {
                parameter = _parameter;
            }
        }

        private ParameterCheck(ParameterCheck check)
        {
            address = check.address;
            parameter = check.parameter;
        }

        public Object Clone()
        {
            return new ParameterCheck(this);
        }

        public void SetParameter(dynamic _parameter)
        {
            parameter = _parameter;
        }

        public bool Check()
        {
            return Program.game.Read(address) == parameter;
        }
    }
}
