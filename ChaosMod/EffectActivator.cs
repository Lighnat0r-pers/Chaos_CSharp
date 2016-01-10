using System;

namespace ChaosMod
{
    public enum ActivationType
    {
        Single = 0,
        Continuous,
        StartOnly,
    }

    public enum DeactivationType
    {
        Standard = 0,
        Never,
    }

    class EffectActivator
    {
        public TimedEffect effect { get; set; }

        private dynamic Target { get; }
        private MemoryAddress Address { get; }
        private ActivationType ActivationType { get; }
        private DeactivationType DeactivationType { get; }

        private dynamic original;

        public EffectActivator(string target, MemoryAddress address, ActivationType activation, DeactivationType deactivation)
        {
            Target = address.ConvertToRightDataType(target);
            Address = address;
            ActivationType = activation;
            DeactivationType = deactivation;
        }

        public void Activate()
        {
            if (effect.IsInactive ||
                (effect.IsSuspended && ActivationType != ActivationType.StartOnly) ||
                ActivationType == ActivationType.Continuous)
            {
                original = Address.Read();
                Address.Write(Target);
            }
        }

        public void Deactivate()
        {

            if (effect.IsActive && DeactivationType != DeactivationType.Never)
            {
                if (original == null)
                {
                    throw new ArgumentNullException("Tried to deactivate activator without original value.");
                }

                Address.Write(original);
            }
        }
    }
}
