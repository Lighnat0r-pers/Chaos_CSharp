using System;
using System.Collections.Generic;

namespace GTAVC_Chaos
{
    class GameVersion
    {
        private int addressValue;
        private SortedList<long, int> offsets;

        public string name;

        public GameVersion(string name, int addressValue, SortedList<long, int> offsets)
        {
            this.name = name;
            this.addressValue = addressValue;
            this.offsets = offsets;

            System.Diagnostics.Debug.WriteLine("Version " + name + " debug address test: " + GetAddressForVersion(0x00A10B50));
        }

        public int GetAddressForVersion(int address)
        {
            long key = FindLastKeySmallerThanOrEqualTo<long>(offsets.Keys, address);

            return address + offsets[key];
        }

        public static T FindLastKeySmallerThanOrEqualTo<T>(IList<T> sortedCollection, T key) where T : IComparable<T>
        {
            int index = FindFirstIndexGreaterThan<T>(sortedCollection, key) - 1;

            if (index < 0)
            {
                throw new Exception("Tried to get address below defined range of offsets.");
            }
            return sortedCollection[index];
        }

        public static int FindFirstIndexGreaterThan<T>(IList<T> sortedCollection, T key) where T : IComparable<T>
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
