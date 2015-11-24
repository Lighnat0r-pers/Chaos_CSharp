using System;

namespace ChaosMod
{
    class EffectActivator
    {
        private dynamic original;

        private string type;
        private dynamic target;
        private MemoryAddress address;

        public EffectActivator(string type, string target, MemoryAddress address)
        {
            this.type = type;
            this.target = address.ConvertToRightDataType(target);
            this.address = address;
        }

        public void Activate()
        {
            switch (type)
            {
                case "simple":
                    original = address.Read();
                    address.Write(target);
                    break;
                default:
                    throw new NotSupportedException("Tried to activate activator with unsupported type.");
            }
        }

        public void Deactivate()
        {
            if (original == null)
            {
                throw new ArgumentNullException(nameof(original), "Tried to deactivate activator without original value set.");
            }

            switch (type)
            {
                case "simple":
                    address.Write(original);
                    break;
                default:
                    throw new NotSupportedException("Tried to deactivate activator with unsupported type.");
            }

            original = null;
        }
    }
}
