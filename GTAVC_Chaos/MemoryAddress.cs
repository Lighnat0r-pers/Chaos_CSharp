using AccessProcessMemory;
using System;
using System.Collections.Generic;

namespace GTAVC_Chaos
{
    class MemoryAddress
    {
        private Memory memory;

        private string baseAddressName;
        public MemoryAddress baseAddress;

        public string name;
        public long address;
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

        public void UpdateForVersion(GameVersion newVersion, GameVersion oldVersion)
        {
            if (newVersion == null || oldVersion == null)
            {
                throw new Exception("Tried to update address for version but version is not defined.");
            }

            address = newVersion.GetAddressForVersion(address, oldVersion);
        }

        public void ResolveBaseAddress()
        {
            if (baseAddressName == null)
            {
                throw new Exception("Tried to resolve base address but no base address name set.");
            }

            baseAddress = Program.game.FindMemoryAddressByName(baseAddressName);
        }

        public void SetMemoryHandle(Memory memory)
        {
            this.memory = memory;
        }

        public dynamic Read()
        {
            if (memory == null)
            {
                throw new Exception("Tried to read an address without a handle to the game process.");
            }

            // NOTE(Ligh): In case of a dynamic address, resolve the address.
            if (baseAddress != null)
            {
                address = GetDynamicAddress();
            }

            return memory.Read(address, type, size);
        }

        public void Write(dynamic input)
        {
            if (memory == null)
            {
                throw new Exception("Tried to write to an address without a handle to the game process.");
            }

            // NOTE(Ligh): In case of a dynamic address, resolve the address.
            if (baseAddress != null)
            {
                address = GetDynamicAddress();
            }

            memory.Write(address, input, type, size);
        }

        /// <summary>
        /// Reads the pointer value in baseAddress and adds offset to it.
        /// As the Read() function calls this function, this works recursively until a static address
        /// is reached, so multiple pointer levels are supported.
        /// </summary>
        /// <returns>
        /// Up to date dynamic address for this function. 
        /// This should always be used immediately and only once to avoid using an outdated address.
        /// </returns>
        private long GetDynamicAddress()
        {
            if (baseAddress == null)
            {
                throw new Exception("Tried to get the dynamic address of a static memory address.");
            }

            if (baseAddress.type != "int" && baseAddress.type != "long")
            {
                throw new Exception("Tried to use an address with a non-pointer type ( " + baseAddress.type + " ) as a pointer address.");
            }

            return baseAddress.Read() + offset;
        }
    }
}
