
namespace GTAVC_Chaos
{
    class EffectActivator
    {
        public string type;
        public dynamic target;
        public MemoryAddress address;

        public EffectActivator(string type, dynamic target, MemoryAddress address)
        {
            this.type = type;
            this.target = target;
            this.address = address;
        }
    }
}
