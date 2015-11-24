using System;

namespace ChaosMod
{
    class ParameterCheck : ICheck
    {
        public MemoryAddress Address { get; private set; }
        public dynamic Parameter { get; set; }

        public ParameterCheck(MemoryAddress address, string parameter = null)
        {
            Address = address;

            if (parameter != null)
            {
                Parameter = address.ConvertToRightDataType(parameter);
            }
        }

        public bool Succeeds()
        {
            if (Parameter == null)
            {
                throw new ArgumentNullException(nameof(Parameter), "No parameter set.");
            }

            return Address.Read() == Parameter;
        }
    }
}
