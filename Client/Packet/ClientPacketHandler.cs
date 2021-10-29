using System;
using ServerCore;
using Google.Protobuf;
using Google.Protobuf.Protocol;


namespace Client
{
    class ClientPacketHandler
    {
        public static void S_EnterOKHandler(PacketSession session, IMessage packet)
        {
            C_Connect connect = new C_Connect();
            connect.PlayerInfo = new PlayerInfo { Name = "아무개", ModelIndex = 1, Pos = new Position { X = 0, Y = 0 } };
           
            session.Send(connect);
        }

        public static void S_SpawnHandler(PacketSession session, IMessage packet)
        {

        }

        public static void S_EnterPlayerHandler(PacketSession session, IMessage packet)
        {

        }

        public static void S_MoveHandler(PacketSession session, IMessage packet)
        {

        }
    }
}
