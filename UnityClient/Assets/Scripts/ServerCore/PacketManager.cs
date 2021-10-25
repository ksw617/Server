using System;
using System.Collections.Generic;
using Google.Protobuf;

namespace ServerCore
{
    public abstract class PacketManager
    {
        protected Dictionary<ushort, Action<PacketSession, IMessage>> handler = new Dictionary<ushort, Action<PacketSession, IMessage>>();
        protected Dictionary<ushort, Action<ushort, PacketSession, ArraySegment<byte>>> onRecv = new Dictionary<ushort, Action<ushort, PacketSession, ArraySegment<byte>>>();

        protected PacketManager()
        {
            Initialize();
        }

        public abstract void Initialize();

        public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer)
        {
            ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + sizeof(ushort));

            Action<ushort, PacketSession, ArraySegment<byte>> action = null;

            if (onRecv.TryGetValue(id, out action))
            {
                action.Invoke(id, session, buffer);
            }
        }

        protected virtual void MakePacket<T>(ushort id, PacketSession session, ArraySegment<byte> buffer) where T : IMessage, new()
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
