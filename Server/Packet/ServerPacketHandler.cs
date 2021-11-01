using System;
using ServerCore;
using Google.Protobuf;
using Google.Protobuf.Protocol;


namespace Server
{
    class ServerPacketHandler
    {
        public static void C_ConnectHandler(PacketSession session, IMessage packet)
        {
            GameRoom room = GameRoomManager.Instance.Find(1);
            Player player = PlayerManager.Instance.Create();
            
            ClientSession clientSession = (ClientSession)session;
            clientSession.MyPlayer = player;
            player.Session = clientSession;

            C_Connect c_Connect = (C_Connect)packet;
            PlayerInfo playerInfo = c_Connect.PlayerInfo;
            player.Info = playerInfo;
            player.RoomID = room.roomID;
           
            //나한테 현재 들어와 있는 애들 뿌려줌
            S_Spawn s_Spawn = new S_Spawn();
            s_Spawn.PlayerInfos.Add(room.GetPlayers());
            session.Send(s_Spawn);

            //다른애들한테 나의 정보를 넣어줌
            room.BroadcastAllPlayer(player);

            //다하고 나는 들어감
            room.Enter(player);

        }

        public static void C_MoveHandler(PacketSession session, IMessage packet)
        {

        }


    }
}
