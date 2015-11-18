using System;
using System.Collections.Generic;

namespace GTAVC_Chaos
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
            long key = FindLastKeySmallerThanOrEqualTo<long>(offsets.Keys, address);

            return offsets[key];
        }

        private static T FindLastKeySmallerThanOrEqualTo<T>(IList<T> sortedCollection, T key) where T : IComparable<T>
        {
            int index = FindFirstIndexGreaterThan<T>(sortedCollection, key) - 1;

            if (index < 0)
            {
                throw new Exception("Tried to get address below defined range of offsets.");
            }
            return sortedCollection[index];
        }

        private static int FindFirstIndexGreaterThan<T>(IList<T> sortedCollection, T key) where T : IComparable<T>
        {
            int begin = 0;
            int end = sortedCollection.Count;
            while (end > begin)
            {
                int index = (begin + end) / 2;
                T el = sortedCollection[index];
                if (el.CompareTo(key) > 0)
                    end = index;
                else
                    begin = index + 1;
            }

            return end;
        }
    }


}
