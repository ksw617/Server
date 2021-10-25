using System;
using ServerCore;
using Google.Protobuf;
using Google.Protobuf.Protocol;

class ClientPacketManager : PacketManager
{
    private static ClientPacketManager instance = new ClientPacketManager();
    public static ClientPacketManager Instance { get { return instance; } }

    public override void Initialize()
    {
        onRecv.Add((ushort)MsgID.SChat, MakePacket<S_Chat>);
        handler.Add((ushort)MsgID.SChat, ClientPacketHandler.S_ChatHandler);

    }

    protected override void MakePacket<T>(ushort id, PacketSession session, ArraySegment<byte> buffer)
    {
        T packet = new T();
        packet.MergeFrom(buffer.Array, buffer.Offset + 4, buffer.Count - 4);

        PacketQueue.Instance.Push(id, packet);
    }

    public Action<PacketSession, IMessage> GetPacketHandler(ushort id)
    {
        Action<PacketSession, IMessage> action = null;
        if (handler.TryGetValue(id, out action))
        {
            return action;
        }

        return null;
    }

}
