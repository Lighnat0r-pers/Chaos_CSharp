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
            if (parameter != null)
            {
                this.parameter = parameter;
            }
        }

        private ParameterCheck(ParameterCheck check)
        {
            this.address = check.address;
            this.parameter = check.parameter;
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
            return Program.game.Read(address) == parameter;
        }
    }
}
