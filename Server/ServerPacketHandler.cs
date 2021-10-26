using System;
using ServerCore;
using Google.Protobuf;
using Google.Protobuf.Protocol;


namespace Server
{
    class ServerPacketHandler
    {
        public static void C_EnterHandler(PacketSession session, IMessage packet)
        {
            C_Enter enter = packet as C_Enter;
            ClientSession clientSession = session as ClientSession;

            Console.WriteLine($"입장 : {enter.PlayerInfo.Name}");

            S_Chat s_Chat = new S_Chat();
            s_Chat.Msg = "서버에 오신거 환영합니다.";

            session.Send(s_Chat);
        }

        public static void C_MoveHandler(PacketSession session, IMessage packet)
        {
            C_Move move = packet as C_Move;
            ClientSession clientSession = session as ClientSession;

        }
    }
}
