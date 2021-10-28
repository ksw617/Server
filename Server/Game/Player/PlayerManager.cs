using System.Collections.Generic;

namespace Server
{
    class PlayerManager
    {
        private static PlayerManager instance = new PlayerManager();
        public static PlayerManager Instance { get { return instance; } }

        int playerID = 0;
        Dictionary<int, Player> players = new Dictionary<int, Player>();
        object lockObj = new object();

        public Player Create()
        {
            lock (lockObj)
            {
                ++playerID;
                Player player = new Player();
                player.PlayerID = playerID;
                players.Add(playerID, player);

                return player;
            }
        }

        public Player Find(int id)
        {
            lock (lockObj)
            {
                Player player = null;
                players.TryGetValue(id, out player);
                return player;
            }
        }

        public void Remove(int id)
        {
            lock(lockObj)
            {
                players.Remove(id);
            }
        }
    }
}
