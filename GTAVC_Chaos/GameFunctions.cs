using System;
using System.Collections.Generic;

namespace GTAVC_Chaos
{
    // NOTE(Ligh): GTAVC game specific functions. Not sure where to put these or what to do with them, so they'll go here for now.
    static class GameFunctions
    {
        static public string GetCurrentMission()
        {
            // TODO(Ligh): Properly get this list from some external file.
            Dictionary<string, string> missionList = new Dictionary<string, string>();
            missionList.Add("AMBULAE", "Paramedic");
            missionList.Add("ASSIN1", "Road Kill");
            missionList.Add("ASSIN2", "Waste The Wife");
            missionList.Add("ASSIN3", "Autocide");
            missionList.Add("ASSIN4", "Check Out At The Check In");
            missionList.Add("ASSIN5", "Loose Ends");
            missionList.Add("BANKJ1", "No Escape");
            missionList.Add("BANKJ2", "The Shootist");
            missionList.Add("BANKJ3", "The Driver");
            missionList.Add("BANKJ4", "The Job");
            missionList.Add("BARON1", "The Chase");
            missionList.Add("BARON2", "Phnom Penh '86");
            missionList.Add("BARON3", "The Fastest Boat");
            missionList.Add("BARON4", "Supply And Demand");
            missionList.Add("BARON5", "Rub Out");
            missionList.Add("BIKE1", "Alloy Wheels Of Steel");
            missionList.Add("BIKE2", "Messing With The Man");
            missionList.Add("BIKE3", "Hog Tied");
            missionList.Add("BMX_1", "Trial By Dirt");
            missionList.Add("BOATBUY", "Buying Boatyard");
            missionList.Add("CAP_1", "Cap The Collector");
            missionList.Add("CARBUY", "Buying Sunshine Autos");
            missionList.Add("CARPAR1", "Cone Crazy");
            missionList.Add("COPCAR", "Vigilante");
            missionList.Add("COUNT1", "Spilling The Beans");
            missionList.Add("COUNT2", "Hit The Courier");
            missionList.Add("CUBAN1", "Stunt Boat Challenge");
            missionList.Add("CUBAN2", "Cannon Fodder");
            missionList.Add("CUBAN3", "Naval Engagement");
            missionList.Add("CUBAN4", "Trojan Voodoo");
            missionList.Add("FINALE", "Keep Your Friends Close");
            missionList.Add("FIRETRK", "Firefighter");
            missionList.Add("GENERA1", "Treacherous Swine");
            missionList.Add("GENERA2", "Mall Shootout");
            missionList.Add("GENERA3", "Guardian Angels");
            missionList.Add("GENERA4", "Sir Yes Sir");
            missionList.Add("GENERA5", "All Hands On Deck");
            missionList.Add("HAIT1", "Juju Scramble");
            missionList.Add("HAIT2", "Bombs Away");
            missionList.Add("HAIT3", "Dirty Lickin's");
            missionList.Add("HOTEL", "An Old Friend");
            missionList.Add("ICECRE1", "Distribution");
            missionList.Add("ICECUT", "Buying Cherry Poppers");
            missionList.Add("INTRO", "In The Beginning");
            missionList.Add("KENT1", "Death Row");
            missionList.Add("KICKSTT", "Dirtring");
            missionList.Add("LAWYER1", "The Party");
            missionList.Add("LAWYER2", "Back Alley Brawl");
            missionList.Add("LAWYER3", "Jury Fury");
            missionList.Add("LAWYER4", "Riot");
            missionList.Add("MIAMI_1", "PCJ Playground");
            missionList.Add("MM", "Bloodring");
            missionList.Add("OVALRIG", "Hotring");
            missionList.Add("PHIL1", "Gun Runner");
            missionList.Add("PHIL2", "Boomshine Saigon");
            missionList.Add("PIZZA", "Pizza Delivery");
            missionList.Add("PORN1", "Recruitment Drive");
            missionList.Add("PORN2", "Dildo Dodo");
            missionList.Add("PORN3", "Martha's Mug Shot");
            missionList.Add("PORN4", "G-Spotlight");
            missionList.Add("PROT1", "Shakedown");
            missionList.Add("PROT2", "Bar Brawl");
            missionList.Add("PROT3", "Cop Land");
            missionList.Add("RACES", "Races");
            missionList.Add("RCHELI1", "RC Raider Pickup");
            missionList.Add("RCPLNE1", "RC Baron Race");
            missionList.Add("RCRACE", "RC Bandit Race");
            missionList.Add("ROCK1", "Love Juice");
            missionList.Add("ROCK2", "Psycho Killer");
            missionList.Add("ROCK3", "Publicity Tour");
            missionList.Add("SERG1", "Four Iron");
            missionList.Add("SERG2", "Two Bit Hit");
            missionList.Add("SERG3", "Demolition Man");
            missionList.Add("TAXI1", "Taxi Driver");
            missionList.Add("TAXICUT", "Buying Kaufman Cabs");
            missionList.Add("TAXIWA1", "V.I.P.");
            missionList.Add("TAXIWA2", "Friendly Rivalry");
            missionList.Add("TAXIWA3", "Cabmaggedon");

            MemoryAddress missionNameAddress = Program.components.findMemoryAddressByName("MissionName");
            return missionList[Program.game.Read(missionNameAddress)];
        }
    }
}
