using System;

namespace GTAVC_Chaos
{
    class GameList
    {
        public Game[] games;

        public GameList(Game[] games)
        {
            this.games = games;
        }

        public Game FindGameByName(string name)
        {
            // TODO(Ligh): Handle not found case.
            return Array.Find(games, p => p.name == name);
        }
    }
}
