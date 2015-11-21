using System;
using System.Collections.Generic;

namespace GTAVC_Chaos
{
    class MemoryAddress
    {
        private Game game;
        private GameVersion gameVersion;
        private long address;

        public string baseAddressName;
        public MemoryAddress baseAddress;

        public string name;
        public long offset;
        public string dataType;
        public int size = 0;

        public bool IsDynamic
        {
            get { return address == 0; }
        }

        public long Address
        {
            get { return IsDynamic ? GetDynamicAddress() : address; }
            set { address = value; }
        }

        /// <summary>
        /// Constructor for static memory address
        /// </summary>
        public MemoryAddress(Game game, string name, long address, GameVersion gameVersion, string type, int length = 0)
            : this(game, name, type, length)
        {
            Address = address;
            this.gameVersion = gameVersion;
        }

        /// <summary>
        /// Constructor for dynamic memory address
        /// </summary>
        public MemoryAddress(Game game, string name, string baseAddressName, long offset, string type, int length = 0)
            : this(game, name, type, length)
        {
            this.baseAddressName = baseAddressName;
            this.offset = offset;
        }

        /// <summary>
        /// Generic constructor
        /// </summary>
        private MemoryAddress(Game game, string name, string dataType, int length = 0)
        {
            this.game = game;
            this.name = name;

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

            if (!sizes.ContainsKey(dataType))
            {
                throw new ArgumentOutOfRangeException(nameof(dataType), "Invalid datatype of memory address.");
            }

            this.dataType = dataType;

            size = sizes[dataType];

            if (size == 0)
            {
                throw new ArgumentException(nameof(size), "Invalid size of memory address.");
            }
        }

        public void UpdateForVersion(GameVersion newVersion)
        {
            // NOTE(Ligh): Dynamic addresses do not depend on the version.
            if (IsDynamic)
            {
                return;
            }

            if (newVersion == null || gameVersion == null)
            {
                throw new ArgumentNullException("Tried to update address for version but version is not defined.");
            }

            if (newVersion != gameVersion)
            {
                address += newVersion.GetOffsetForVersion(address) - gameVersion.GetOffsetForVersion(address);
            }
        }

        public dynamic ConvertToRightDataType(string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input), "Error while converting input for memory, no input");

            dynamic result;
            switch (dataType)
            {
                case "bool":
                    result = Convert.ToBoolean(input);
                    break;
                case "byte":
                    result = Convert.ToByte(input);
                    break;
                case "short":
                    result = Convert.ToInt16(input);
                    break;
                case "int":
                    result = Convert.ToInt32(input);
                    break;
                case "long":
                    result = Convert.ToInt64(input);
                    break;
                case "float":
                    result = Convert.ToSingle(input);
                    break;
                case "double":
                    result = Convert.ToDouble(input);
                    break;
                case "ascii":
                    result = input;
                    break;
                case "unicode":
                    result = input;
                    break;
                default:
                    throw new NotSupportedException($"Tried to convert input to unknown data type {dataType}");
            }

            return result;
        }

        public dynamic Read()
        {
            if (game.memory == null)
            {
                throw new InvalidOperationException("Tried to read an address without a handle to the game process.");
            }

            return game.memory.Read(Address, dataType, size);
        }

        public void Write(dynamic input)
        {
            if (game.memory == null)
            {
                throw new InvalidOperationException("Tried to write to an address without a handle to the game process.");
            }

            game.memory.Write(Address, input, dataType, size);
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
                throw new NotSupportedException("Tried to get the dynamic address of a static memory address.");
            }

            if (baseAddress.dataType != "int" && baseAddress.dataType != "long")
            {
                throw new ArgumentOutOfRangeException(nameof(dataType), $"Tried to use an address with a non-pointer datatype ({baseAddress.dataType}) as a pointer address.");
            }

            return baseAddress.Read() + offset;
        }
    }
}
