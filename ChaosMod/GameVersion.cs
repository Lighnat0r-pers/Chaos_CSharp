using System.Linq;
using System.Collections.Generic;

namespace ChaosMod
{
    class GameVersion
    {
        public string Name { get; }
        public int VersionAddressValue { get; }

        private SortedList<long, int> Offsets { get; }

        public GameVersion(string name, int versionAddressValue, SortedList<long, int> offsets)
        {
            Name = name;
            VersionAddressValue = versionAddressValue;
            Offsets = offsets;
        }

        public long GetOffsetForVersion(long address)
        {
            return Offsets.FirstOrDefault(kvp => kvp.Key >= address).Value;
        }
    }
}
