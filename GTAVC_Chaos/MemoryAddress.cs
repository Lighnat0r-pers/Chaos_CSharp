using System;
using System.Collections.Generic;

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
            // TODO(Ligh): Handle size differently so we don't have to spend time
            // creating this dictionary again for every memory address.
            Dictionary<string, int> sizes = new Dictionary<string, int>()
            {
                {"bool", sizeof(bool)},
                {"byte", sizeof(byte)},
                {"short", sizeof(short)},
                {"int", sizeof(int)},
                {"long", sizeof(long)},
                {"float", sizeof(float)},
                {"double", sizeof(double)},
                {"ascii", length},
                {"unicode", length * 2},
            };

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

            baseAddress = Program.components.FindMemoryAddressByName(baseAddressName);
        }
    }
}
