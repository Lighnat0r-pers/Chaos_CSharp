using System;
using System.Collections.Generic;
using AccessProcessMemory;

namespace ChaosMod
{
    class MemoryAddress
    {
        public MemoryAddress BaseAddress { get; set; }

        public long Address
        {
            get { return IsDynamic ? GetDynamicAddress() : address; }
            set { address = value; }
        }

        public string BaseAddressName { get; }
        public string Name { get; }

        public bool IsDynamic => address == 0;

        private Game Game { get; }
        private GameVersion GameVersion { get; }
        private long Offset { get; }
        private DataType DataType { get; }
        private int Size { get; }

        private long address;

        /// <summary>
        /// Constructor for static memory address.
        /// </summary>
        public MemoryAddress(Game game, string name, long address, GameVersion gameVersion, DataType type, int length = 0)
            : this(game, name, type, length)
        {
            if (gameVersion == null)
            {
                throw new ArgumentNullException(nameof(gameVersion), "Unable to construct memory address: No game version set.");
            }

            Address = address;
            GameVersion = gameVersion;
        }

        /// <summary>
        /// Constructor for dynamic memory address.
        /// </summary>
        public MemoryAddress(Game game, string name, string baseAddressName, long offset, DataType type, int length = 0)
            : this(game, name, type, length)
        {
            BaseAddressName = baseAddressName;
            Offset = offset;
        }

        /// <summary>
        /// Generic constructor
        /// </summary>
        private MemoryAddress(Game game, string name, DataType dataType, int length = 0)
        {
            Name = name;
            Game = game;

            // TODO(Ligh): Handle size differently so we don't have to spend time creating this dictionary again for every memory address.
            var sizes = new Dictionary<DataType, int>()
            {
                {DataType.Bool, sizeof(bool)},
                {DataType.Byte, sizeof(byte)},
                {DataType.Short, sizeof(short)},
                {DataType.Int, sizeof(int)},
                {DataType.Long, sizeof(long)},
                {DataType.Float, sizeof(float)},
                {DataType.Double, sizeof(double)},
                {DataType.Ascii, length},
                {DataType.Unicode, length * 2},
            };

            if (!sizes.ContainsKey(dataType))
            {
                throw new ArgumentOutOfRangeException(nameof(dataType), "Invalid datatype of memory address.");
            }

            DataType = dataType;

            Size = sizes[dataType];

            if (Size == 0)
            {
                throw new ArgumentException(nameof(Size), "Invalid size of memory address.");
            }
        }

        public void UpdateForVersion(GameVersion newVersion)
        {
            // NOTE(Ligh): Dynamic addresses do not depend on the version.
            if (IsDynamic)
            {
                return;
            }

            if (newVersion == null || GameVersion == null)
            {
                throw new ArgumentNullException("Tried to update address for version but version is not defined.");
            }

            if (newVersion != GameVersion)
            {
                address += newVersion.GetOffsetForVersion(address) - GameVersion.GetOffsetForVersion(address);
            }
        }

        public dynamic ConvertToRightDataType(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input), "Error while converting input for memory, no input");
            }

            dynamic result;
            switch (DataType)
            {
                case DataType.Bool:
                    result = Convert.ToBoolean(input);
                    break;
                case DataType.Byte:
                    result = Convert.ToByte(input);
                    break;
                case DataType.Short:
                    result = Convert.ToInt16(input);
                    break;
                case DataType.Int:
                    result = Convert.ToInt32(input);
                    break;
                case DataType.Long:
                    result = Convert.ToInt64(input);
                    break;
                case DataType.Float:
                    result = Convert.ToSingle(input);
                    break;
                case DataType.Double:
                    result = Convert.ToDouble(input);
                    break;
                case DataType.Ascii:
                    result = input;
                    break;
                case DataType.Unicode:
                    result = input;
                    break;
                default:
                    throw new NotSupportedException($"Tried to convert input to unknown data type {DataType}");
            }

            return result;
        }

        public dynamic Read()
        {
            if (Game.Memory == null)
            {
                throw new InvalidOperationException("Tried to read an address without a handle to the game process.");
            }

            return Game.Memory.Read(Address, DataType, Size);
        }

        public void Write(dynamic input)
        {
            if (Game.Memory == null)
            {
                throw new InvalidOperationException("Tried to write to an address without a handle to the game process.");
            }

            Game.Memory.Write(Address, input, DataType, Size);
        }

        /// <summary>
        /// Determines the currently valid address for a dynamic address.
        /// </summary>
        /// <returns>
        /// Up to date dynamic address for this function. 
        /// This should always be used immediately and only once to avoid using an outdated address.
        /// </returns>
        private long GetDynamicAddress()
        {
            if (BaseAddress == null)
            {
                throw new InvalidOperationException("Tried to get the dynamic address of a static memory address.");
            }

            // TODO(Ligh): Switch to using IntPtr for pointers.
            if (BaseAddress.DataType != DataType.Int && BaseAddress.DataType != DataType.Long)
            {
                throw new ArgumentOutOfRangeException(nameof(DataType), $"Tried to use an address with a non-pointer datatype ({BaseAddress.DataType}) as a pointer address.");
            }

            return BaseAddress.Read() + Offset;
        }
    }
}
