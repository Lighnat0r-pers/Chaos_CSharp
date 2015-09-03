
namespace GTAVC_Chaos
{
    class EffectActivator
    {
        public string type;
        public object target;
        public MemoryAddress address;

        public EffectActivator(string type, string target, MemoryAddress address)
        {
            this.type = type;
            this.target = target;
            this.address = address;
        }
    }
}
