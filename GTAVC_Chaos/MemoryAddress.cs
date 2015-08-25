using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GTAVC_Chaos
{
    class MemoryAddress
    {
        public string name;

        public long address;

        private string baseAddressName;
        public MemoryAddress baseAddress;

        public long offset;

        public string type;
        public int size = 0;

        /// <summary>
        /// Constructor for static memory address
        /// </summary>
        public MemoryAddress(string _name, long _address, string _type, int length = 0)
            : this(_name, _type, length)
        {
            address = _address;
        }

        /// <summary>
        /// Constructor for dynamic memory address
        /// </summary>
        public MemoryAddress(string _name, string _baseAddressName, long _offset, string _type, int length = 0)
            : this(_name, _type, length)
        {
            baseAddressName = _baseAddressName;
            offset = _offset;
        }

        /// <summary>
        /// Generic constructor
        /// </summary>
        private MemoryAddress(string _name, string _type, int length = 0)
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

        public void ResolveBaseAddress()
        {
            if (baseAddressName == null)
            {
                throw new Exception("Tried to resolve base address but no base address name set.");
            }

            baseAddress = Program.components.findMemoryAddressByName(baseAddressName);
        }
    }
}
