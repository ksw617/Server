using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ServerCore;
using System.Threading;

namespace Server
{
    class Program
    {
        static Listener listener;
        static void AcceptHandler(Socket clientSocket)
        {
            try
            {
                //session 초기화
                Session session = new Session();
                session.Initialize(clientSocket);

                Console.WriteLine("손님 입장");
                byte[] sendBuffer = Encoding.UTF8.GetBytes("어서옵쇼~!");
                session.Send(sendBuffer);

                Thread.Sleep(100);

                session.Disconnect();

           
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            IPHostEntry iPHostEntry = Dns.GetHostEntry(host);
            IPAddress iPAddress = iPHostEntry.AddressList[0];

            Console.WriteLine(iPHostEntry.AddressList[0]);
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 777);

            listener = new Listener();
            listener.Initialize(iPEndPoint, AcceptHandler);

            while (true)
            {

            }

        }
    }
}
