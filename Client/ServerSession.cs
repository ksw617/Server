using System;
using System.Net;
using ServerCore;
using Google.Protobuf;
using Google.Protobuf.Protocol;

namespace Client
{
    class ServerSession : PacketSession
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");

            C_Chat chat = new C_Chat();
            chat.Chat = "Hello Server";

            ushort size = (ushort)chat.CalculateSize();
            ushort id = (ushort)MsgID.CChat;
            byte[] sendBuffer = new byte[size + 4];

            Array.Copy(BitConverter.GetBytes(size + 4), 0, sendBuffer, 0, sizeof(ushort));
            Array.Copy(BitConverter.GetBytes(id), 0, sendBuffer, 2, sizeof(ushort));
            Array.Copy(chat.ToByteArray(), 0, sendBuffer, 4, size);
           

            Send(new ArraySegment<byte>(sendBuffer));

        }

        public override void OnReceivePacket(ArraySegment<byte> buffer)
        {
            ClientPacketManager.Instance.OnRecvPacket(this, buffer);
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
