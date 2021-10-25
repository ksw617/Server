using System;
using System.Net;
using UnityEngine;
using System.Collections.Generic;
using ServerCore;
using Google.Protobuf;
using Google.Protobuf.Protocol;

public class NetworkManager : MonoBehaviour
{
    static NetworkManager instance = null;
    public static NetworkManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<NetworkManager>();
            }

            return instance;
        }
      
    }


    ServerSession serverSession = new ServerSession();
    public GameObject obj;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }
    void Start()
    {
        string host = Dns.GetHostName();
        IPHostEntry iPHostEntry = Dns.GetHostEntry(host);
        IPAddress iPAddress = iPHostEntry.AddressList[0];
        IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 777);

        Connector connector = new Connector();
        connector.Initialize(iPEndPoint, () => { return serverSession; });

    }

    public void Update()
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

    public void CreateBox()
    {
        Instantiate(obj);
    }
}
