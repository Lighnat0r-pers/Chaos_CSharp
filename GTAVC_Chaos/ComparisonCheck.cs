using System;

namespace GTAVC_Chaos
{
    class ComparisonCheck : ICheck
    {
        public MemoryAddress[] addresses;
        public bool equal;

        public ComparisonCheck(MemoryAddress[] _addresses, dynamic _equal)
        {
            addresses = _addresses;
            equal = _equal;
        }

        private ComparisonCheck(ComparisonCheck check)
        {
            addresses = check.addresses;
            equal = check.equal;
        }

        public Object Clone()
        {
            return new ComparisonCheck(this);
        }

        public bool Check()
        {
            bool result = true;
            dynamic value = Program.game.Read(addresses[0]);
            for (int i = 1; i < addresses.Length; i++)
            {
                result = result && (value == Program.game.Read(addresses[0]));
            }

            return result == equal;
        }
    }
}
