using System;
using System.Collections.Generic;
using ServerCore;
using Google.Protobuf;
using Google.Protobuf.Protocol;


namespace Client
{
    class ClientPacketManager
    {
        private static ClientPacketManager instance = new ClientPacketManager();
        public static ClientPacketManager Instance { get { return instance; } }

        Dictionary<ushort, Action<PacketSession, IMessage>> handler = new Dictionary<ushort, Action<PacketSession, IMessage>>();
        Dictionary<ushort, Action<ushort, PacketSession, ArraySegment<byte>>> onRecv = new Dictionary<ushort, Action<ushort, PacketSession, ArraySegment<byte>>>();
        ClientPacketManager()
        {
            Initialize();
        }

        public void Initialize()
        {
            onRecv.Add((ushort)MsgID.SChat, MakePacket<S_Chat>);
            handler.Add((ushort)MsgID.SChat, ClientPacketHandler.S_ChatHandler);

        }

        public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer)
        {
            //[size[2]][ID[2]][][][][][]...
            ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + sizeof(ushort));

            //action 얘를 null로 초기화
            Action<ushort, PacketSession, ArraySegment<byte>> action = null;

            if (onRecv.TryGetValue(id, out action))
            {
                action.Invoke(id, session, buffer);
            }
        }

        void MakePacket<T>(ushort id, PacketSession session, ArraySegment<byte> buffer) where T : IMessage, new()
        {
            T packet = new T();
            packet.MergeFrom(buffer.Array, buffer.Offset + 4, buffer.Count - 4);
            Action<PacketSession, IMessage> action = null;
            if (handler.TryGetValue(id, out action))
            {
                action.Invoke(session, packet);
            }
        }
    }
}
