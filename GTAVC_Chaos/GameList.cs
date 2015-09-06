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
            Game result = null;
            foreach (Game game in games)
            {
                if (game.name == name)
                {
                    result = game;
                    break;
                }
            }

            if (result == null)
            {
                throw new Exception("Game " + name + " not found.");
            }

            return result;
        }
    }
}
