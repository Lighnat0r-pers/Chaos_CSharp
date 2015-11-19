using System;
using System.Collections.Generic;

namespace GTAVC_Chaos
{
    class ComparisonCheck : ICheck
    {
        private List<string> addressNames;
        public List<MemoryAddress> addresses;
        public bool equal;

        public ComparisonCheck(List<string> addressNames, bool equal)
        {
            this.addressNames = addressNames;
            this.equal = equal;
        }

        private ComparisonCheck(ComparisonCheck check)
        {
            this.addressNames = check.addressNames;
            this.equal = check.equal;
            this.ResolveReferences();
        }

        public Object Clone()
        {
            return new ComparisonCheck(this);
        }

        public bool Check()
        {
            dynamic value = addresses[0].Read();
            bool result = addresses.TrueForAll(c => c.Read() == value);
            return result == equal;
        }

        public void ResolveReferences()
        {
            addresses = new List<MemoryAddress>();

            foreach (var addressName in addressNames)
            {
                var address = Program.game.FindMemoryAddressByName(addressName);

                if (address == null)
                {
                    throw new ArgumentOutOfRangeException("address", "Memory address for comparison check is not defined.");
                }

                addresses.Add(address);
            }
        }
    }
}
