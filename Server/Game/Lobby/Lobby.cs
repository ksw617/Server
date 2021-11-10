using System;
using System.Collections.Generic;

namespace Server
{
    public class Lobby
    {
        public int lobbyID { get; set; }
        private object lockObj = new object();
        private Dictionary<int, GameRoom> gameRooms = new Dictionary<int, GameRoom>();
        private Dictionary<int, Player> players = new Dictionary<int, Player>();

        public void Update()
        {

        }

        public void Enter(Player player)
        {
            lock (lockObj)
            {
                players.Add(player.PlayerID, player);
                Console.WriteLine($"{ player.Info.Name}님이 입장 하였습니다.");
            }
        }
        

        public void CreateGameRoom(int id)
        {
            lock (lockObj)
            {
                GameRoom gameRoom = GameRoomManager.Instance.Create();
               
                if(players.ContainsKey(id))
                {
                    players.Remove(id, out Player player);
                    player.RoomID = gameRoom.roomID;
                    gameRoom.Enter(player);            
                }
            }
        }
    }
}
