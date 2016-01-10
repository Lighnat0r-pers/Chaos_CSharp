using System;
using System.Collections.Generic;

namespace ChaosMod
{
    class ComparisonCheck : ICheck
    {
        private List<MemoryAddress> Addresses { get; }
        private bool Equal { get; }

        public ComparisonCheck(List<MemoryAddress> addresses, bool equal)
        {
            Addresses = addresses;
            Equal = equal;
        }

        public bool Succeeds()
        {
            dynamic value = Addresses[0].Read();
            bool result = Addresses.TrueForAll(c => c.Read() == value);
            return result == Equal;
        }
    }
}
