using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;

namespace ChaosMod
{
    public delegate void EffectEventHandler(object sender, EventArgs e);

    class EffectScripter
    {
        // TODO(Ligh): Clean this up.
        private dynamic addresses = new ExpandoObject();

        private dynamic vars = new ExpandoObject();

        private TimedEffect Effect { get; }

        public EffectScripter(TimedEffect effect)
        {
            Effect = effect;

            // TODO(Ligh): Get the script name and script from the effect.

            // TODO(Ligh): Clean this up.
            foreach (var address in Settings.Game.MemoryAddresses)
            {
                ((IDictionary<string, object>)addresses).Add(address.Name, address);
            }

            // TODO(Ligh): Use the script code instead of these hardcoded debug methods.
            Effect.OnInit += new EventHandler(OnInit);
            Effect.OnActivate += new EventHandler(OnActivate);
            Effect.OnUpdate += new EventHandler(OnUpdate);
            Effect.OnDeactivate += new EventHandler(OnDeactivate);
            Effect.OnSuspend += new EventHandler(OnSuspend);
        }

        public void OnInit(object sender, EventArgs e)
        {
            Debug.WriteLine("OnInit Raised");

            if (Effect.Name == "Interior")
            {
                vars.targetInterior = Settings.Random.Next(17);
                if (vars.targetInterior == 13)
                {
                    // Interior 13 does not exist.
                    vars.targetInterior = 18;
                }

                vars.originalInterior = 0;
                addresses.CurrentInterior.Write(vars.targetInterior);
            }
        }

        public void OnActivate(object sender, EventArgs e)
        {
            Debug.WriteLine("OnActivate Raised");

            if (Effect.Name == "Interior")
            {
                addresses.CurrentInterior.Write(vars.targetInterior);
            }
        }

        public void OnUpdate(object sender, EventArgs e)
        {
            //Debug.WriteLine("OnUpdate Raised");

            if (Effect.Name == "Interior")
            {
                addresses.CurrentInterior.Write(vars.targetInterior);
            }
        }

        public void OnDeactivate(object sender, EventArgs e)
        {
            Debug.WriteLine("OnDeactivate Raised");

            if (Effect.Name == "Interior")
            {
                addresses.CurrentInterior.Write(vars.originalInterior);
            }
        }

        public void OnSuspend(object sender, EventArgs e)
        {
            Debug.WriteLine("OnSuspend Raised");

            if (Effect.Name == "Interior")
            {
                addresses.CurrentInterior.Write(vars.originalInterior);
            }
        }

        private void DebugReadAddresses()
        {
            foreach (var property in (IDictionary<string, object>)addresses)
            {
                var address = property.Value as MemoryAddress;
                Debug.WriteLine($"{property.Key}: {address.Read() as object}");
            }
        }
    }
}
