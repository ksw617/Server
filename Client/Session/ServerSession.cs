using System;
using System.Net;
using ServerCore;
using Google.Protobuf.Protocol;

namespace Client
{
    class ServerSession : PacketSession
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");


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
