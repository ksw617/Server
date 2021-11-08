using System.Collections.Generic;

namespace Server
{
    public class LobbyManager
    {
        private static LobbyManager instance = new LobbyManager();
        public static LobbyManager Instance { get { return instance; } }

        int lobbyID = 0;
        Dictionary<int, Lobby> lobbies = new Dictionary<int, Lobby>();
        object lockObj = new object();

        public Lobby Create()
        {
            lock (lockObj)
            {
                ++lobbyID;
                Lobby lobby = new Lobby();
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
                lobbies.Remove(id);
            }
        }
    }
}
