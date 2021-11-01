using System.Collections.Generic;

namespace Server
{
    class GameRoomManager
    {
        private static GameRoomManager instance = new GameRoomManager();
        public static GameRoomManager Instance { get { return instance; } }

        int roomID = 0;
        Dictionary<int, GameRoom> gameRooms = new Dictionary<int, GameRoom>();
        object lockObj = new object();

        public GameRoom Create()
        {
            lock (lockObj)
            {
                ++roomID;
                GameRoom gameRoom = new GameRoom();
                gameRoom.roomID = roomID;
                gameRooms.Add(roomID, gameRoom);

                return gameRoom;
            }
        }

        public GameRoom Find(int id)
        {
            lock (lockObj)
            {
                GameRoom gameRoom = null;
                gameRooms.TryGetValue(id, out gameRoom);
                return gameRoom;
            }
        }

        public void Remove(int id)
        {
            lock (lockObj)
            {
                gameRooms.Remove(id);
            }
        }
    }
}
