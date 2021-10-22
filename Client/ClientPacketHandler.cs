using System;
using ServerCore;
using Google.Protobuf;
using Google.Protobuf.Protocol;


namespace Client
{
    class ClientPacketHandler
    {
        public static void S_ChatHandler(PacketSession session, IMessage packet)
        {
            S_Chat chat = packet as S_Chat;
            ServerSession serverSession = session as ServerSession;

            Console.WriteLine(chat.Chat);
        }
    }
}
