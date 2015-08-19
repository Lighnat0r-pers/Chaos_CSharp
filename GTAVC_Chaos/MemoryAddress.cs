using System;

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
        /// <param name="_name"></param>
        /// <param name="_address"></param>
        /// <param name="_type"></param>
        /// <param name="_size"></param>
        public MemoryAddress(string _name, long _address, string _type, int _size = 0)
        {

            name = _name;
            address = _address;
            type = _type;

            switch (type)
            {
                case "bool":
                    size = 1;
                    break;
                case "byte":
                    size = 1;
                    break;
                case "short":
                    size = 2;
                    break;
                case "int":
                    size = 4;
                    break;
                case "long":
                    size = 8;
                    break;
                case "float":
                    size = 4;
                    break;
                case "double":
                    size = 8;
                    break;
                case "ascii":
                    size = _size;
                    break;
                case "unicode":
                    size = _size;
                    break;
            }

            if (size == 0)
            {
                throw new Exception("Invalid type or size of memory address.");
            }
        }
    }
}
