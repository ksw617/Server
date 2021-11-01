using System;
using System.Net;
using ServerCore;
using Google.Protobuf.Protocol;


namespace Server
{
    class ClientSession : PacketSession
    {
        public int SessionID { get; set; }
        public Player MyPlayer { get; set; }
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");
            S_EnterOk s_EnterOK = new S_EnterOk();
            Send(s_EnterOK);
        }

        public override void OnReceivePacket(ArraySegment<byte> buffer)
        {
            ServerPacketManager.Instance.OnRecvPacket(this, buffer);
        }

        public override void OnSend(int numberOfBytes)
        {
            
        }
        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected : {endPoint}");
            SessionManager.Instance.Remove(SessionID); // Session메니저에서 지꺼 제거
            PlayerManager.Instance.Remove(MyPlayer.PlayerID); // Player메니저에서 해당 플레이어 제거
            GameRoom gameRoom = GameRoomManager.Instance.Find(MyPlayer.RoomID);
            gameRoom.Leave(MyPlayer.PlayerID); // Player가 있던 방에 Player 없앰.


        }

    }
}
