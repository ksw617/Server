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

            C_Move c_Move = new C_Move();
            c_Move.Pos = new Position();
            Random random = new Random();

            while (true)
            {
                Thread.Sleep(1000);
                c_Move.Pos.X = random.Next(0, 100);
                c_Move.Pos.Y = random.Next(0, 100);

                serverSession.Send(c_Move);

            }
        }
    }
}