using System.Collections.Generic;

namespace Server
{
    class SessionManager
    {
        private static SessionManager instance = new SessionManager();
        public static SessionManager Instance { get { return instance; } }

        int sessionID = 0;
        Dictionary<int, ClientSession> sessions = new Dictionary<int, ClientSession>();
        object lockObj = new object();

        public ClientSession Create()
        {
            lock (lockObj)
            {
                ++sessionID;

                ClientSession session = new ClientSession();
                session.SessionID = sessionID;
                sessions.Add(sessionID, session);

                return session;
            }
        }

        public ClientSession Find(int id)
        {
            lock (lockObj)
            {
                ClientSession session = null;
                sessions.TryGetValue(id, out session);
                return session;
            }
        }

        public void Remove(int id)
        {
            lock (lockObj)
            {
                sessions.Remove(id);
            }
        }
    }
}
