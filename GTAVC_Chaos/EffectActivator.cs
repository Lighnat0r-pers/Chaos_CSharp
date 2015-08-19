
namespace GTAVC_Chaos
{
    class EffectActivator
    {
        public string type;
        public object target;
        public MemoryAddress address;

        public EffectActivator(string _type, string _target, string _address)
        {
            type = _type;
            target = _target;
            address = Program.components.findMemoryAddressByName(_address);
        }
    }
}
