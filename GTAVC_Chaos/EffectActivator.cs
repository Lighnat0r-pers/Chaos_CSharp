using System;

namespace GTAVC_Chaos
{
    class EffectActivator
    {
        private dynamic original = null;

        public string type;
        public dynamic target;
        public MemoryAddress address;

        public EffectActivator(string type, dynamic target, MemoryAddress address)
        {
            this.type = type;
            this.target = target;
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
                    throw new Exception("Tried to activate activator with unsupported type.");
            }
        }

        public void Deactivate()
        {
            if (original == null)
            {
                throw new Exception("Tried to deactivate activator without original value set.");
            }

            switch (type)
            {
                case "simple":
                    address.Write(original);
                    break;
                default:
                    throw new Exception("Tried to deactivate activator with unsupported type.");
            }

            original = null;
        }
    }
}
