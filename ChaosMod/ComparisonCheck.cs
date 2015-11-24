using System;
using System.Collections.Generic;

namespace ChaosMod
{
    class ComparisonCheck : ICheck
    {
        private List<MemoryAddress> addresses;
        private bool equal;

        public ComparisonCheck(List<MemoryAddress> addresses, bool equal)
        {
            this.addresses = addresses;
            this.equal = equal;
        }

        public bool Succeeds()
        {
            dynamic value = addresses[0].Read();
            bool result = addresses.TrueForAll(c => c.Read() == value);
            return result == equal;
        }
    }
}
