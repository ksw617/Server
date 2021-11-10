using System.Collections.Generic;

namespace Server
{
    class GameRoomManager
    {
        private static GameRoomManager instance = new GameRoomManager();
        public static GameRoomManager Instance { get { return instance; } }

        int roomID = 0;
        Dictionary<int, GameRoom> gameRooms = new Dictionary<int, GameRoom>();
        Queue<GameRoom> gameRoomPool = new Queue<GameRoom>();
        object lockObj = new object();

        public GameRoom Create()
        {
            lock (lockObj)
            {
                ++roomID;
                GameRoom gameRoom = GetGameRoom();
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
                gameRoomPool.Enqueue(gameRooms[id]);
                gameRooms.Remove(id);              
            }
        }

        GameRoom GetGameRoom()
        {
            if (gameRoomPool.Count > 0)
            {
                return gameRoomPool.Dequeue();
            }

            return new GameRoom();
        }
    }
}
