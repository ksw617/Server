using System;
using System.Net;
using ServerCore;
using Google.Protobuf.Protocol;

class ServerSession : PacketSession
{
    public override void OnConnected(EndPoint endPoint)
    {
        Console.WriteLine($"OnConnected : {endPoint}");

        C_Chat chat = new C_Chat();
        chat.Chat = "Hello Server";

        Send(chat);

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
