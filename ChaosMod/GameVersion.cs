using System;
using System.Linq;
using System.Collections.Generic;

namespace ChaosMod
{
    class GameVersion
    {
        private SortedList<long, int> offsets;

        public string name;
        public int versionAddressValue;

        public GameVersion(string name, int versionAddressValue, SortedList<long, int> offsets)
        {
            this.name = name;
            this.versionAddressValue = versionAddressValue;
            this.offsets = offsets;
        }

        public long GetOffsetForVersion(long address)
        {
            return offsets.First(kvp => kvp.Key >= address).Value;
        }
    }
}
