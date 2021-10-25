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
            onRecv.Add((ushort)MsgID.CChat, MakePacket<C_Chat>);
            handler.Add((ushort)MsgID.CChat, ServerPacketHandler.C_ChatHandler);
            
        }
    }
}
