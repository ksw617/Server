using ServerCore;
using Google.Protobuf.Protocol;


namespace Client
{
    class ClientPacketManager : PacketManager
    {
        private static ClientPacketManager instance = new ClientPacketManager();
        public static ClientPacketManager Instance { get { return instance; } }

        public override void Initialize()
        {
            onRecv.Add((ushort)MsgID.SChat, MakePacket<S_Chat>);
            handler.Add((ushort)MsgID.SChat, ClientPacketHandler.S_ChatHandler);

        }

    }
}
