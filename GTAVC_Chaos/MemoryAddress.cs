using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GTAVC_Chaos
{
    class MemoryAddress
    {
        public string name;
        public long address;
        public string type;
        public int size = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        public MemoryAddress(string _name, long _address, string _type, int length = 0)
        {
            Dictionary<string, int> sizes = new Dictionary<string, int>();
            sizes.Add("bool", sizeof(bool));
            sizes.Add("byte", sizeof(byte));
            sizes.Add("short", sizeof(short));
            sizes.Add("int", sizeof(int));
            sizes.Add("long", sizeof(long));
            sizes.Add("float", sizeof(float));
            sizes.Add("double", sizeof(double));
            sizes.Add("ascii", length);
            sizes.Add("unicode", length * 2);

            name = _name;
            address = _address;
            if (!sizes.ContainsKey(_type))
            {
                throw new Exception("Invalid type of memory address.");
            }

            type = _type;

            size = sizes[type];

            if (size == 0)
            {
                throw new Exception("Invalid size of memory address.");
            }
        }
    }
}
