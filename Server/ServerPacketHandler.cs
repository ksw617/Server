using System;
using ServerCore;
using Google.Protobuf;
using Google.Protobuf.Protocol;


namespace Server
{
    class ServerPacketHandler
    {
        public static void C_ChatHandler(PacketSession session, IMessage packet)
        {
            C_Chat chat = packet as C_Chat;
            ClientSession clientSession = session as ClientSession;

            Console.WriteLine(chat.Chat);
        }
    }
}
