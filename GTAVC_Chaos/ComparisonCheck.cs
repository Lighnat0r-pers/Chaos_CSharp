using System;
using System.Collections.Generic;

namespace GTAVC_Chaos
{
    class ComparisonCheck : ICheck
    {
        public List<MemoryAddress> addresses;
        public bool equal;

        public ComparisonCheck(List<MemoryAddress> addresses, bool equal)
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
            dynamic value = addresses[0].Read();
            bool result = addresses.TrueForAll(c => c.Read() == value);
            return result == equal;
        }
    }
}
