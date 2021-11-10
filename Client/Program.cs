using System.Net;
using ServerCore;
using System.Threading;
using System;
using Google.Protobuf.Protocol;

namespace Client
{
    class Program
    {
        static Connector connector = new Connector();
        static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            IPHostEntry iPHostEntry = Dns.GetHostEntry(host);
            IPAddress iPAddress = iPHostEntry.AddressList[0];
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 777);

            ServerSession serverSession = new ServerSession();

            connector.Initialize(iPEndPoint, () => { return serverSession; });

            while (true)
            {
                Thread.Sleep(1000);
         

            }
        }
    }
}