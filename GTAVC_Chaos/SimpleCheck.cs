using System;

namespace GTAVC_Chaos
{
    // TODO(Ligh): This class is actually identical to the ParameterCheck class except that the value isn't edited. 
    // Use the other class instead of this one. Should these be combined in the xml as well?
    class SimpleCheck : ICheck
    {
        public MemoryAddress address;
        public dynamic value;

        public SimpleCheck(MemoryAddress address, dynamic value)
        {
            this.address = address;
            this.value = value;
        }

        public bool Check()
        {
            return address.Read() == value;
        }
    }
}
