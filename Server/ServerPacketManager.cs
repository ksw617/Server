using ServerCore;
using Google.Protobuf.Protocol;

namespace Server
{
    class ServerPacketManager : PacketManager
    {
       private static ServerPacketManager instance = new ServerPacketManager();
       public static ServerPacketManager Instance { get { return instance; }  }

        public override void Initialize()
        {
            onRecv.Add((ushort)MsgID.CEnter, MakePacket<C_Enter>);
            handler.Add((ushort)MsgID.CEnter, ServerPacketHandler.C_EnterHandler);
            onRecv.Add((ushort)MsgID.CMove, MakePacket<C_Move>);
            handler.Add((ushort)MsgID.CMove, ServerPacketHandler.C_MoveHandler);

        }
    }
}
