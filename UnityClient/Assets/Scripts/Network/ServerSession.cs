using System;
using System.Net;
using ServerCore;
using Google.Protobuf.Protocol;

class ServerSession : PacketSession
{
    public override void OnConnected(EndPoint endPoint)
    {
        Console.WriteLine($"OnConnected : {endPoint}");

        C_Enter c_Enter = new C_Enter();
        PlayerInfo playerInfo = new PlayerInfo();
        playerInfo.PlayerId = 1;
        playerInfo.Name = "아무개";
        c_Enter.PlayerInfo = playerInfo;


        Send(c_Enter);

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
