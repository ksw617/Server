using System;
using System.Net;

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


            listener.Initialize(iPEndPoint, () => { return new ClientSession(); });

            while (true)
            {

            }

        }
    }
}
