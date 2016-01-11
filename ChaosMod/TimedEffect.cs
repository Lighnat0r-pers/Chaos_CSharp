using System;
using System.Collections.Generic;

namespace ChaosMod
{
    public enum ActivationState
    {
        Inactive = 0,
        Active = 1,
        Suspended = 2,
    }

    public enum EffectType
    {
        Inline = 0,
        Script = 1,
    }

    class TimedEffect
    {
        static private long defaultEffectLength => 30000;

        /// <summary>
        /// Duration of the effect taking time multipliers into account.
        /// </summary>
        public long EffectLength
        {
            get { return effectLength; }
            set { effectLength = value * Settings.TimeMultiplier; }
        }
        private long effectLength;

        public string Category { get; set; }
        public int Difficulty { get; set; }

        public ActivationState Activation { get; private set; }

        public string Name { get; }
        public string ScriptName { get; }
        public EffectType EffectType { get; }

        public bool CanActivate => Limitations?.TrueForAll(l => l.Check()) ?? true; // TODO(Ligh): Properly set this for scripted effects.
        public bool IsInactive => Activation == ActivationState.Inactive;
        public bool IsActive => Activation == ActivationState.Active;
        public bool IsSuspended => Activation == ActivationState.Suspended;

        private List<EffectActivator> Activators { get; }
        private List<Limitation> Limitations { get; }

        private EffectScripter Scripter { get; }

        public event EventHandler OnInit;
        public event EventHandler OnActivate;
        public event EventHandler OnUpdate;
        public event EventHandler OnSuspend;
        public event EventHandler OnDeactivate;


        /// <summary>
        /// Constructor for a script based timed effect.
        /// </summary>
        public TimedEffect(string name, string category, int difficulty, string script, uint duration = 0)
            : this(name, category, difficulty, duration)
        {
            EffectType = EffectType.Script;

            ScriptName = script;
            Scripter = new EffectScripter(this);
        }

        /// <summary>
        /// Constructor for an inline timed effect.
        /// </summary>
        public TimedEffect(string name, string category, int difficulty, List<EffectActivator> activators, uint duration = 0, List<Limitation> limitations = null)
            : this(name, category, difficulty, duration)
        {
            // TODO(Ligh): Do we want to keep inline effects if we have scripted effects?
            // Inline effects work, but only for simple effects that could very easily be scripted.
            // Having both will increase the odds of bugs and be potentially confusing to anyone wanting to add effects.
            EffectType = EffectType.Inline;

            Activators = activators;
            Limitations = limitations;

            foreach (var activator in activators)
            {
                activator.effect = this;
            }
        }

        /// <summary>
        /// Base constructor
        /// </summary>
        private TimedEffect(string name, string category, int difficulty, uint duration)
        {
            EffectLength = duration == 0 ? defaultEffectLength : duration;

            Name = name;
            Category = category;
            Difficulty = difficulty;
        }

        public void Init()
        {
            if (EffectType == EffectType.Inline)
            {
                Activate();
            }
            else
            {
                OnInit(this, EventArgs.Empty);
            }

            Activation = ActivationState.Active;
        }

        public void Activate()
        {
            if (EffectType == EffectType.Inline)
            {
                foreach (var activator in Activators)
                {
                    activator.Activate();
                }
            }
            else
            {
                OnActivate(this, EventArgs.Empty);
            }

            Activation = ActivationState.Active;
        }

        public void Update()
        {
            if (EffectType == EffectType.Inline)
            {
                Activate();
            }
            else
            {
                OnUpdate(this, EventArgs.Empty);
            }

            Activation = ActivationState.Active;
        }

        public void Deactivate()
        {
            if (EffectType == EffectType.Inline)
            {
                foreach (var activator in Activators)
                {
                    activator.Deactivate();
                }
            }
            else
            {
                OnDeactivate(this, EventArgs.Empty);
            }

            Activation = ActivationState.Inactive;
        }

        public void Suspend()
        {
            if (EffectType == EffectType.Inline)
            {
                Deactivate();
            }
            else
            {
                OnSuspend(this, EventArgs.Empty);
            }

            Activation = ActivationState.Suspended;
        }
    }
}
