using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Program
    {
        static Connector connector;

        static void ConnectdHandler(Socket clientSocket)
        {
     
            try
            {
                byte[] recvBuffer = new byte[1024];
                int recvSize = clientSocket.Receive(recvBuffer);
                string recvData = Encoding.UTF8.GetString(recvBuffer, 0, recvSize);
                Console.WriteLine(recvData);

                //보낸다
                //문자열-> data
                byte[] sendBuffer = Encoding.UTF8.GetBytes("오랜만이시네요.");
                clientSocket.Send(sendBuffer);

                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
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