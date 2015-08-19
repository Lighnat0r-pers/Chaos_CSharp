
namespace GTAVC_Chaos
{
    static class TimedEffectLimitations
    {

        static public bool IsOnFoot()
        {
            return true;
        }

        static public bool IsInVehicle()
        {
            return true;
        }

        static public bool IsInBoat()
        {
            return true;
        }

        static public bool IsInVehicleNotCar()
        {
            return true;
        }

        static public bool IsOnMission(string missionName = null)
        {
            if (missionName == null)
                return true;
            return true;
        }

        static public bool IsOnNotRealMission()
        {
            return true;
        }

        static public bool IsOnFootNormalStatus()
        {
            return true;
        }

        static public bool IsNormalStatus()
        {
            return true;
        }

        static public bool IsPermanentEffectActive(string permanentEffect)
        {
            return true;
        }

        static public bool IsInVehicleLowHealth(string healthString)
        {
            float health = float.Parse(healthString);
            return true;
        }

        static public bool IsInInterior()
        {
            return true;
        }

        static public bool HasWeapon(string weaponName = "All")
        {
            return true;
        }

    }
}
