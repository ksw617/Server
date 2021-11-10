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
            Player player = PlayerManager.Instance.Create();
            ClientSession clientSession = (ClientSession)session;
            clientSession.MyPlayer = player;
            player.Session = clientSession;

            Lobby lobby = LobbyManager.Instance.Find(1);
            lobby.Enter(player);
        }

        public static void C_CreateRoomHandler(PacketSession session, IMessage packet)
        {
            Lobby lobby = LobbyManager.Instance.Find(1);
            ClientSession clientSession = (ClientSession)session;
            lobby.CreateGameRoom(clientSession.MyPlayer.PlayerID);
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
