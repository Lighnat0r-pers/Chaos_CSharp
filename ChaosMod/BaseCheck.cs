using System;

namespace ChaosMod
{
    public enum FailType
    {
        Abort,
        Suspend,
    }

    class BaseCheck : ICheck
    {
        public FailType FailType { get; }

        private MemoryAddress Address { get; }
        private dynamic FailCase { get; }

        public BaseCheck(MemoryAddress address, string failCase, FailType failType)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address), "Invalid address for base check");
            }

            Address = address;
            FailCase = address.ConvertToRightDataType(failCase);

            FailType = failType;
        }

        public bool Succeeds()
        {
            return Address.Read() != FailCase;
        }
    }
}
