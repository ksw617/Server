using Google.Protobuf.Protocol;

namespace Server
{
    public class Player
    {
        public bool RoomOwener { get; set; }
        public int PlayerID { get; set; }
        public int RoomID { get; set; }
        
        public ClientSession Session { get; set; }
        public PlayerInfo Info { get; set; }
    }
}
