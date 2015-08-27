using System;

namespace GTAVC_Chaos
{
    class SimpleCheck : ICheck
    {
        public MemoryAddress address;
        public dynamic value;

        public SimpleCheck(MemoryAddress _address, dynamic _value)
        {
            address = _address;
            value = _value;
        }

        private SimpleCheck(SimpleCheck check)
        {
            address = check.address;
            value = check.value;
        }

        public Object Clone()
        {
            return new SimpleCheck(this);
        }

        public bool Check()
        {
            return Program.game.Read(address) == value;
        }
    }
}
