using ServerCore;
using Google.Protobuf.Protocol;

namespace Server
{
    class ServerPacketManager : PacketManager
    {
        private static ServerPacketManager instance = new ServerPacketManager();
        public static ServerPacketManager Instance { get { return instance; } }

        public override void Initialize()
        {
            onRecv.Add((ushort)MsgID.CCreatePlayer, MakePacket<C_CreatePlayer>);
            handler.Add((ushort)MsgID.CCreatePlayer, ServerPacketHandler.C_CreatePlayerHandler);

            onRecv.Add((ushort)MsgID.CEnterGameroom, MakePacket<C_EnterGameRoom>);
            handler.Add((ushort)MsgID.CEnterGameroom, ServerPacketHandler.C_EnterGameRoomHandler);


        }
    }
}
