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
            onRecv.Add((ushort)MsgID.SConnected, MakePacket<S_Connected>);
            handler.Add((ushort)MsgID.SConnected, ClientPacketHandler.S_ConnectedHandler);
           
            onRecv.Add((ushort)MsgID.SEnter, MakePacket<S_Enter>);
            handler.Add((ushort)MsgID.SEnter, ClientPacketHandler.S_EnterHandler);

        }

    }
}
