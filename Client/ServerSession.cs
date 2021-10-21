using System;
using System.Net;
using ServerCore;
using System.Text;
using Google.Protobuf;
using Google.Protobuf.Examples.AddressBook;
using static Google.Protobuf.Examples.AddressBook.Person.Types;

namespace Client
{
    class ServerSession : PacketSession
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");

            Person john = new Person
            {
                Id = 1234,
                Name = "John Doe",
                Email = "jdoe@example.com",
                Phones = { new PhoneNumber { Number = "555-4321", Type = PhoneType.Home } }
            };

            //(ushort) : int -> ushort
            //사이즈 받음
            ushort size = (ushort)john.CalculateSize();
            //우선 사이즈 추가된 만큼 배열을 넓혀줌
            byte[] sendBuffer = new byte[size + 2];
            //sendBuffer : [][][][][][][][][][][]
            //{[][]} <- 여기에 (크기 + 2)를 BitConverter.GetBytes로 바이트로 변환
            //sizeof(ushort) : 2 
            Array.Copy(BitConverter.GetBytes(size + 2), 0, sendBuffer, 0, sizeof(ushort));
            //sendBuffer : [size[2]]{[][][][][][][][][]}
            //{} 이부분에 내가 원하는 자료를 넣어줌
            Array.Copy(john.ToByteArray(), 0, sendBuffer, 2, size);
           
            //보냄
            Send(new ArraySegment<byte>(sendBuffer));

        }

        public override void OnReceivePacket(ArraySegment<byte> buffer)
        {
            string recvData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
            Console.WriteLine(recvData);
        }

        public override void OnSend(int numberOfBytes)
        {
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

    }
}
