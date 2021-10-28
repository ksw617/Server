using System;
using ServerCore;
using Google.Protobuf;
using Google.Protobuf.Protocol;


namespace Server
{
    class ServerPacketHandler
    {
        public static void C_CreatePlayerHandler(PacketSession session, IMessage packet)
        {
            C_CreatePlayer createPlayer = packet as C_CreatePlayer;

            Player player = PlayerManager.Instance.Create();
            player.Info = createPlayer.PlayerInfo;

            S_Enter enter = new S_Enter();
            session.Send(enter);
 
        }

        public static void C_EnterGameRoomHandler(PacketSession session, IMessage packet)
        {

            Console.WriteLine("방 입장");

        }


    }
}
