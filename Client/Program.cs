using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using ServerCore;
using System.Threading;

namespace Client
{
    class Program
    {
        static Connector connector;

        static void ConnectdHandler(Socket clientSocket)
        {
     
            try
            {
                //session 초기화
                Session session = new Session();
                session.Initialize(clientSocket);

                //보낸다
                byte[] sendBuffer = Encoding.UTF8.GetBytes("오랜만이시네요.");
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
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 777);


            connector = new Connector();
            connector.Initialize(iPEndPoint, ConnectdHandler);

            while (true)
            { 
            }
        }
    }
}