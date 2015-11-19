using System;

namespace GTAVC_Chaos
{
    // TODO(Ligh): This class is actually identical to the ParameterCheck class except that the value isn't edited. 
    // Use the other class instead of this one. Should these be combined in the xml as well?
    class SimpleCheck : ICheck
    {
        private string addressName;
        public MemoryAddress address;
        public dynamic value;

        public SimpleCheck(string addressName, dynamic value)
        {
            this.addressName = addressName;
            this.value = value;
        }

        private SimpleCheck(SimpleCheck check)
        {
            this.addressName = check.addressName;
            this.value = check.value;
            this.ResolveReferences();
        }

        public Object Clone()
        {
            return new SimpleCheck(this);
        }

        public bool Check()
        {
            return address.Read() == value;
        }

        public void ResolveReferences()
        {
            address = Program.game.FindMemoryAddressByName(addressName);

            if (address == null)
            {
                throw new ArgumentOutOfRangeException("address", "Memory address for simple check is not defined.");
            }

            value = address.ConvertToRightDataType(value);
        }
    }
}
