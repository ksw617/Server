using System;
using System.Collections.Generic;
using ServerCore;
using Google.Protobuf;
using Google.Protobuf.Protocol;

namespace Server
{
    class ServerPacketManager
    {
       private static ServerPacketManager instance = new ServerPacketManager();
       public static ServerPacketManager Instance { get { return instance; }  }

        Dictionary<ushort, Action<PacketSession, IMessage>> handler = new Dictionary<ushort, Action<PacketSession, IMessage>>();
        //    key : id, value :  void(ushort id, PacketSession session, ArraySegment<byte> buffer)
        Dictionary<ushort, Action<ushort, PacketSession, ArraySegment<byte>>> onRecv = new Dictionary<ushort, Action<ushort, PacketSession, ArraySegment<byte>>>();
        ServerPacketManager()
        {
            Initialize();
        }

        public void Initialize()
        {
            //Key : id, value : Action -> MaketPacket<T> 실행시켜줄
            onRecv.Add((ushort)MsgID.CChat, MakePacket<C_Chat>);
            handler.Add((ushort)MsgID.CChat, ServerPacketHandler.C_ChatHandler);
            
        }

        public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer)
        {
            //[size[2]][ID[2]][][][][][]...
            ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + sizeof(ushort));

            //action 얘를 null로 초기화
            Action<ushort, PacketSession, ArraySegment<byte>> action = null;
            //onRecv <- key값이 id인 아이가 있다면 action -> MakePacket<T> ex) if key : MsgID.CChat -> T == C_Chat
            //ex) action == MakePacket<C_Chat>
            if (onRecv.TryGetValue(id, out action))
            {
                //ex) MakePacket<C_Chat>(id, session, buffer)
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
