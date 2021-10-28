using System;
using ServerCore;
using Google.Protobuf;
using Google.Protobuf.Protocol;


namespace Client
{
    class ClientPacketHandler
    {
        public static void S_ConnectedHandler(PacketSession session, IMessage packet)
        {
            Console.WriteLine("접속됨");

            C_CreatePlayer c_CreatePlayer = new C_CreatePlayer();
            c_CreatePlayer.PlayerInfo = new PlayerInfo();
            c_CreatePlayer.PlayerInfo.Name = "아무개";
            c_CreatePlayer.PlayerInfo.ModelIndex = 1;

            session.Send(c_CreatePlayer);
        }

        public static void S_EnterHandler(PacketSession session, IMessage packet)
        {
           // C_EnterGameRoom enterGameRoom = new C_EnterGameRoom();
           // session.Send(enterGameRoom);
        }
    }
}
