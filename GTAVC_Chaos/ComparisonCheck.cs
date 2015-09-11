using System;

namespace GTAVC_Chaos
{
    class ComparisonCheck : ICheck
    {
        public MemoryAddress[] addresses;
        public bool equal;

        public ComparisonCheck(MemoryAddress[] addresses, dynamic equal)
        {
            this.addresses = addresses;
            this.equal = equal;
        }

        private ComparisonCheck(ComparisonCheck check)
        {
            this.addresses = check.addresses;
            this.equal = check.equal;
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
                result = result && (value == Program.game.Read(addresses[i]));
            }

            return result == equal;
        }
    }
}
