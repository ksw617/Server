using System;
using System.Collections.Generic;

namespace Server
{
    public class Lobby
    {
        public int lobbyID { get; set; }
        private object lockObj = new object();
        private Dictionary<int, GameRoom> gameRooms = new Dictionary<int, GameRoom>();

        public void Update()
        {

        }

        public void CreateGameRoom(GameRoomInfo gameRoomInfo)
        {
            GameRoom gameRoom = GameRoomManager.Instance.Create();
            gameRoom.gameRoomInfo = gameRoomInfo;
        }
    }
}
