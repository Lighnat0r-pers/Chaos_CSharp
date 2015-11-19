using System;

namespace GTAVC_Chaos
{
    interface ICheck : ICloneable
    {
        bool Check();
        void ResolveReferences();
    }
}
