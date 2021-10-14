using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class Program
    {
        static Listener listener;

        //받았을때 호출할 함수
        static void AcceptHandler(Socket clientSocket)
        {
            try
            {
                Console.WriteLine("손님 입장");
                byte[] sendBuffer = Encoding.UTF8.GetBytes("어서옵쇼~!");
                clientSocket.Send(sendBuffer);

                byte[] recvBuffer = new byte[1024];
                int recvSize = clientSocket.Receive(recvBuffer);
                string recvData = Encoding.UTF8.GetString(recvBuffer, 0, recvSize);
                Console.WriteLine(recvData);

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

            Console.WriteLine(iPHostEntry.AddressList[0]);
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 777);

            //Listener 생성
            listener = new Listener();
            //Listener 초기화(주소::입구, 받았을때 호출할 함수);
            listener.Initialize(iPEndPoint, AcceptHandler);

            while (true)
            {

            }

        }
    }
}
