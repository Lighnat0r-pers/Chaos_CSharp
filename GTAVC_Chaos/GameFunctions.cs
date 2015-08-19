using System;
using System.Collections.Generic;

namespace GTAVC_Chaos
{
    // NOTE(Ligh): GTAVC game specific functions. Not sure where to put these or what to do with them, so they'll go here for now.
    static class GameFunctions
    {
        static public string GetCurrentMission()
        {
            Dictionary<string, string> missionList = new Dictionary<string, string>();
            MemoryAddress missionNameAddress = Program.components.findMemoryAddressByName("MissionName");
            return Program.game.Read(missionNameAddress);
        }
    }
}
