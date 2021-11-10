using System.Collections.Generic;

namespace Server
{
    public class LobbyManager
    {
        private static LobbyManager instance = new LobbyManager();
        public static LobbyManager Instance { get { return instance; } }

        int lobbyID = 0;
        Dictionary<int, Lobby> lobbies = new Dictionary<int, Lobby>();
        Queue<Lobby> lobbyPool = new Queue<Lobby>();
        object lockObj = new object();

        public Lobby Create()
        {
            lock (lockObj)
            {
                ++lobbyID;
                Lobby lobby = GetLobby();
                lobby.lobbyID = lobbyID;
                lobbies.Add(lobbyID, lobby);

                return lobby;
            }
        }

        public Lobby Find(int id)
        {
            lock (lockObj)
            {
                Lobby lobby = null;
                lobbies.TryGetValue(id, out lobby);
                return lobby;
            }
        }

        public void Remove(int id)
        {
            lock (lockObj)
            {
                lobbyPool.Enqueue(lobbies[id]);
                lobbies.Remove(id);
            }
        }

        Lobby GetLobby()
        {
            if (lobbyPool.Count > 0)
            {
                return lobbyPool.Dequeue();
            }

            return new Lobby();
        }
    }
}
