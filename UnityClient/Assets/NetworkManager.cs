using System;
using System.Net;
using UnityEngine;
using System.Collections.Generic;
using ServerCore;
using Google.Protobuf;

public class NetworkManager : MonoBehaviour
{
    private ServerSession serverSession = new ServerSession();
    void Start()
    {
        string host = Dns.GetHostName();
        IPHostEntry iPHostEntry = Dns.GetHostEntry(host);
        IPAddress iPAddress = iPHostEntry.AddressList[0];
        IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 777);

        Connector connector = new Connector();
        connector.Initialize(iPEndPoint, () => { return serverSession; });
        
    }

    private void Update()
    {
        List<Packet> list = PacketQueue.Instance.PopAll();
        foreach (Packet packet in list)
        {
            Action<PacketSession, IMessage> handler = ClientPacketManager.Instance.GetPacketHandler(packet.Id);
            if (handler != null)
            {
                handler.Invoke(serverSession, packet.Message);
            }
        }
    }

}
