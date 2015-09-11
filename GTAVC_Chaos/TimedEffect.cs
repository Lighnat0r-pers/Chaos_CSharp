
namespace GTAVC_Chaos
{
    class TimedEffect : BaseEffect
    {
        static private int defaultEffectLength = 30000;

        private EffectActivator[] activators;
        private Limitation[] limitations;

        /// <summary>
        /// Property effectLength which automatically takes the timeMultiplier as defined in Settings.cs into account.
        /// </summary>
        public int effectLength
        {
            get { return length; }
            set { length = value * Settings.timeMultiplier; }
        }
        private int length;

        public TimedEffect(string name, string category, int difficulty, EffectActivator[] activators, int duration = 0, Limitation[] limitations = null)
            : base(name, category, difficulty)
        {
            effectLength = (duration == 0) ? defaultEffectLength : duration;

            this.activators = activators;
            this.limitations = limitations;
        }

        public bool CanActivate()
        {
            bool canActivate = true;
            // Check all limitations, if any one returns true the effect cannot activate.
            foreach (Limitation limitation in limitations)
            {
                if (!limitation.Check())
                {
                    canActivate = false;
                    break;
                }
            }

            return canActivate;
        }

        public override void Activate()
        {
            if (!CanActivate())
            {
                return;
            }

            foreach (EffectActivator activator in activators)
            {
                activator.Activate();
            }
        }

        public override void Deactivate()
        {
            foreach (EffectActivator activator in activators)
            {
                activator.Deactivate();
            }
        }

        public void CalculateMTTH()
        {
            float activationTime = effectLength / difficulty;
        }
    }
}
