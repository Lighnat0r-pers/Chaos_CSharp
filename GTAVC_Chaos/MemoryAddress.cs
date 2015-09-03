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
        public MemoryAddress(string name, long address, string type, int length = 0)
            : this(name, type, length)
        {
            this.address = address;
        }

        /// <summary>
        /// Constructor for dynamic memory address
        /// </summary>
        public MemoryAddress(string name, string baseAddressName, long offset, string type, int length = 0)
            : this(name, type, length)
        {
            this.baseAddressName = baseAddressName;
            this.offset = offset;
        }

        /// <summary>
        /// Generic constructor
        /// </summary>
        private MemoryAddress(string name, string type, int length = 0)
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

            this.name = name;

            if (!sizes.ContainsKey(type))
            {
                throw new Exception("Invalid type of memory address.");
            }

            this.type = type;

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
