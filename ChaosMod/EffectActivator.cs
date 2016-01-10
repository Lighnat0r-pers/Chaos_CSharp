using System;

namespace ChaosMod
{
    class EffectActivator
    {
        static public Activation DEFAULT_ACTIVATION => Activation.Single;
        static public Deactivation DEFAULT_DEACTIVATION => Deactivation.Standard;

        public enum Activation
        {
            Single = 1,
            Continuous = 2,
            StartOnly = 3,
        }

        public enum Deactivation
        {
            Standard = 1,
            Never = 2,
        }

        public TimedEffect effect { get; set; }

        private dynamic original;

        private string type;
        private dynamic target;
        private MemoryAddress address;
        private Activation activation;
        private Deactivation deactivation;

        public EffectActivator(string type, string target, MemoryAddress address, Activation activation, Deactivation deactivation)
        {
            this.type = type;
            this.target = address.ConvertToRightDataType(target);
            this.address = address;
            this.activation = activation;
            this.deactivation = deactivation;

        }

        public void Activate()
        {
            if (effect.IsInactive ||
                (effect.IsSuspended && activation != Activation.StartOnly) ||
                activation == Activation.Continuous)
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
        }

        public void Deactivate()
        {

            if (effect.IsActive && deactivation != Deactivation.Never)
            {
                if (original == null)
                {
                    throw new ArgumentNullException("Tried to deactivate activator without original value.");
                }

                switch (type)
                {
                    case "simple":
                        address.Write(original);
                        break;
                    default:
                        throw new NotSupportedException("Tried to deactivate activator with unsupported type.");
                }
            }
        }
    }
}
