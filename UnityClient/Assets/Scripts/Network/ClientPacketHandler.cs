using ServerCore;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using UnityEngine;
class ClientPacketHandler
{
    public static void S_ChatHandler(PacketSession session, IMessage packet)
    {
        S_Chat chat = packet as S_Chat;
        ServerSession serverSession = session as ServerSession;

        Debug.Log(chat.Msg);

    }

    public static void S_MoveHandler(PacketSession session, IMessage packet)
    {
        S_Move move = packet as S_Move;
        ServerSession serverSession = session as ServerSession;

        Debug.Log("work");

    }
}
