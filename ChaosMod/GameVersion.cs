using System.Linq;
using System.Collections.Generic;

namespace ChaosMod
{
    class GameVersion
    {
        private SortedList<long, int> offsets;

        public string name { get; private set; }
        public int versionAddressValue { get; private set; }

        public GameVersion(string name, int versionAddressValue, SortedList<long, int> offsets)
        {
            this.name = name;
            this.versionAddressValue = versionAddressValue;
            this.offsets = offsets;
        }

        public long GetOffsetForVersion(long address)
        {
            return offsets.FirstOrDefault(kvp => kvp.Key >= address).Value;
        }
    }
}
