using Google.Protobuf;
using Google.Protobuf.Protocol;
using System.Collections.Generic;

namespace Server
{
    public class GameRoomInfo
    {

    }
    class GameRoom
    {
        public int roomID { get; set; }
        private object lockObj = new object();
        private Dictionary<int, Player> players = new Dictionary<int, Player>();

        public GameRoomInfo gameRoomInfo { get; set; }

        public void Update()
        {
        }

        public void Enter(Player player)
        {
            lock (lockObj)
            {
                players.Add(player.PlayerID, player);
                System.Console.WriteLine($"{ player.Info.Name}님이 입장 하였습니다.");
            }
        }

        public void Leave(int id)
        {
            lock (lockObj)
            {
                System.Console.WriteLine($"이름 : {players[id].Info.Name}씨 나감");
                if (players[id].RoomOwener)
                {
                    players[id].RoomOwener = false;
                    DestroyGameRoom();
                }
                else
                {
                    players.Remove(id);
                }     
            
            }
        }

        public List<PlayerInfo> GetPlayers()
        {         
            lock (lockObj)
            {
                List<PlayerInfo> playerInfos = new List<PlayerInfo>();
                foreach (var player in players)
                {
                    playerInfos.Add(player.Value.Info);
                }

                return playerInfos;

            }
        }

        public void BroadCast(IMessage packet)
        {
            lock (lockObj)
            {
                foreach (var player in players)
                {
                    player.Value.Session.Send(packet);
                }
            }
        }

        void DestroyGameRoom()
        {
            foreach (var player in players)
            {
                //퇴출
            }

            players.Clear();
            GameRoomManager.Instance.Remove(roomID);
        }

    }
}
