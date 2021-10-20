using System;
using System.Net;
using ServerCore;
using System.Text;
using System.Threading;

namespace Server
{
    class ClientSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");

            byte[] sendBuffer = Encoding.UTF8.GetBytes("Welcome to Server"); 
            Send(new ArraySegment<byte>(sendBuffer, 0, sendBuffer.Length));


            Thread.Sleep(3000);
            Disconnect();
        }
 
        public override int OnReceive(ArraySegment<byte> buffer)
        {
            string recvData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
            Console.WriteLine(recvData);
           
            //지금은 그냥 읽은 만큼 반환
            return buffer.Count;
        }

        public override void OnSend(int numberOfBytes)
        {
            Console.WriteLine($"OnSend : {numberOfBytes} byte");
        }
        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

    }
}
