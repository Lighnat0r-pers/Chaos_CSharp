using System;

namespace GTAVC_Chaos
{
    class SimpleCheck : ICheck
    {
        public MemoryAddress address;
        public dynamic value;

        public SimpleCheck(MemoryAddress address, dynamic value)
        {
            this.address = address;
            this.value = value;
        }

        private SimpleCheck(SimpleCheck check)
        {
            this.address = check.address;
            this.value = check.value;
        }

        public Object Clone()
        {
            return new SimpleCheck(this);
        }

        public bool Check()
        {
            return address.Read() == value;
        }
    }
}
