using System;
using System.Net;
using ServerCore;

namespace Server
{
    class Program
    {
        static Listener listener = new Listener();
        static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            IPHostEntry iPHostEntry = Dns.GetHostEntry(host);
            IPAddress iPAddress = iPHostEntry.AddressList[0];

            Console.WriteLine(iPHostEntry.AddressList[0]);
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 777);


            listener.Initialize(iPEndPoint, () => { return SessionManager.Instance.Create(); });

            while (true)
            {

            }

        }
    }
}
