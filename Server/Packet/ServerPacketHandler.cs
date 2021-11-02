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

            S_Spawn s_Spawn = new S_Spawn();
            s_Spawn.PlayerInfos.Add(room.GetPlayers());
            session.Send(s_Spawn);

            //바꿈
            S_EnterPlayer enterPlayer = new S_EnterPlayer();
            enterPlayer.PlayerID = player.PlayerID;
            enterPlayer.PlayerInfo = player.Info;
            room.BroadCast(enterPlayer);


            room.Enter(player);

        }

        public static void C_MoveHandler(PacketSession session, IMessage packet)
        {
            C_Move c_Move = (C_Move)packet;
            ClientSession clientSession = (ClientSession)session;
            Console.WriteLine($"{clientSession.MyPlayer.Info.Name}가 (X : {c_Move.Pos.X}, Y : {c_Move.Pos.Y}) 로 움직임");

            GameRoom room = GameRoomManager.Instance.Find(clientSession.MyPlayer.RoomID);
            S_Move move = new S_Move();

            //바꿈
            move.PlayerID = clientSession.MyPlayer.PlayerID;
            move.Pos = c_Move.Pos;
            room.BroadCast(move);
        }


    }
}
