using System;
using System.Net;
using ServerCore;
using System.Text;
using System.Threading;
using Google.Protobuf; // 사용
using Google.Protobuf.Examples.AddressBook; // 사용

namespace Server
{
    class ClientSession : PacketSession
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");

            byte[] sendBuffer = Encoding.UTF8.GetBytes("Welcome to Server"); 
            Send(new ArraySegment<byte>(sendBuffer, 0, sendBuffer.Length));


            Thread.Sleep(3000);
            Disconnect();
        }

        public override void OnReceivePacket(ArraySegment<byte> buffer)
        {

            //앞부분 사이즈를 받아와서
            ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);

            Person person = new Person();
            ArraySegment<byte> packet = new ArraySegment<byte>(buffer.Array, buffer.Offset + 2, size - 2);
            person.MergeFrom(packet);

            //확인
            Console.WriteLine($"Name : {person.Name}, Email : {person.Email}");

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
