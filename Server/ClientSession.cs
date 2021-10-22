using System;
using System.Net;
using ServerCore;
using Google.Protobuf;
using Google.Protobuf.Protocol;

namespace Server
{
    class ClientSession : PacketSession
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");

            S_Chat s_Chat = new S_Chat();
            s_Chat.Chat = "This is Server";

            ushort size = (ushort)s_Chat.CalculateSize();
            ushort id = (ushort)MsgID.SChat;
            byte[] sendBuffer = new byte[size + 4];

            Array.Copy(BitConverter.GetBytes(size + 4), 0, sendBuffer, 0, sizeof(ushort));
            Array.Copy(BitConverter.GetBytes(id), 0, sendBuffer, 2, sizeof(ushort));
            Array.Copy(s_Chat.ToByteArray(), 0, sendBuffer, 4, size);

            Send(new ArraySegment<byte>(sendBuffer));



        }

        public override void OnReceivePacket(ArraySegment<byte> buffer)
        {
            ServerPacketManager.Instance.OnRecvPacket(this, buffer);
        }

        public override void OnSend(int numberOfBytes)
        {
            
        }
        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

    }
}
