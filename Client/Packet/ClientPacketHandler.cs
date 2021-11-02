using System;
using ServerCore;
using Google.Protobuf;
using Google.Protobuf.Protocol;


namespace Client
{
    class ClientPacketHandler
    {
        public static void S_EnterOKHandler(PacketSession session, IMessage packet)
        {
            C_Connect connect = new C_Connect();
            Random random = new Random();
            int index = random.Next(0, 100);
            connect.PlayerInfo = new PlayerInfo { Name = $"아무개 : {index}", ModelIndex = 1, Pos = new Position { X = 0, Y = 0 } };
           
            session.Send(connect);
        }

        //내가 들어 갔을때 기존에 있던 애들 뿌려줌.
        public static void S_SpawnHandler(PacketSession session, IMessage packet)
        {
            S_Spawn spawn = (S_Spawn)packet;

            foreach (var playerInfo in spawn.PlayerInfos)
            {
                Console.WriteLine(playerInfo.Name);
            }
        }

        //새로운애가 추가 되었을경우
        public static void S_EnterPlayerHandler(PacketSession session, IMessage packet)
        {
            S_EnterPlayer enterPlayer = (S_EnterPlayer)packet;
        }

        public static void S_MoveHandler(PacketSession session, IMessage packet)
        {
            S_Move move = new S_Move();
            move = (S_Move)packet;
            Console.WriteLine($"Player {move.PlayerID} 가 X : {move.Pos.X}, Y : {move.Pos.Y} 로 움직임");
        }
    }
}
