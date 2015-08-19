using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GTAVC_Chaos
{
    class MemoryAddress
    {

        public string name;
        public long address;
        public Type type;
        public int size = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_address"></param>
        /// <param name="_type"></param>
        /// <param name="_size"></param>
        public MemoryAddress(string _name, long _address, string _type, int _size = 0)
        {
            Dictionary<string, Type> types = new Dictionary<string, Type>();
            types.Add("bool", typeof(bool));
            types.Add("byte", typeof(byte));
            types.Add("short", typeof(short));
            types.Add("int", typeof(int));
            types.Add("long", typeof(long));
            types.Add("float", typeof(float));
            types.Add("double", typeof(double));
            types.Add("ascii", typeof(string));
            types.Add("unicode", typeof(string));

            name = _name;
            address = _address;
            if (!types.ContainsKey(_type))
            {
                throw new Exception("Invalid type of memory address.");
            }

            type = types[_type];

            if (_type == "ascii")
            {
                size = _size;
            }
            else if (_type == "unicode")
            {
                size = _size * 2;
            }
            else
            {
                size = Marshal.SizeOf(type);
            }

            if (size == 0)
            {
                throw new Exception("Invalid size of memory address.");
            }
        }
    }
}
