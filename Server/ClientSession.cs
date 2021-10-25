using System;
using System.Net;
using ServerCore;
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

            Send(s_Chat);

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
