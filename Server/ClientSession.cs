using System;
using System.Net;
using ServerCore;
using System.Text;

namespace Server
{
    class ClientSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");

            byte[] sendBuffer = Encoding.UTF8.GetBytes("Welcome to Server"); 
            Send(new ArraySegment<byte>(sendBuffer, 0, sendBuffer.Length));
        }
 
        public override void OnReceive(ArraySegment<byte> buffer)
        {
            string recvData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
            Console.WriteLine(recvData);
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
