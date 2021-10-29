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
            onRecv.Add((ushort)MsgID.SEnterOk, MakePacket<S_EnterOk>);
            handler.Add((ushort)MsgID.SEnterOk, ClientPacketHandler.S_EnterOKHandler);
           
            onRecv.Add((ushort)MsgID.SSpawn, MakePacket<S_Spawn>);
            handler.Add((ushort)MsgID.SSpawn, ClientPacketHandler.S_SpawnHandler);


            onRecv.Add((ushort)MsgID.SEnterPlayer, MakePacket<S_EnterPlayer>);
            handler.Add((ushort)MsgID.SEnterPlayer, ClientPacketHandler.S_EnterPlayerHandler);


            onRecv.Add((ushort)MsgID.SMove, MakePacket<S_Move>);
            handler.Add((ushort)MsgID.SMove, ClientPacketHandler.S_MoveHandler);

        }

    }
}
