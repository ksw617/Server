using System;
using Google.Protobuf;
using Google.Protobuf.Protocol;

namespace ServerCore
{
    public abstract class PacketSession : Session
    {
        readonly int packetSize = sizeof(ushort);
        public sealed override int OnReceive(ArraySegment<byte> buffer) 
        {
            int processLength = 0;

            while (true)
            {    
                if (buffer.Count < packetSize)
                {
                    break;
                }

                ushort dataSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
                if (buffer.Count < dataSize)
                {
                    break;
                }
                OnReceivePacket(new ArraySegment<byte>(buffer.Array, buffer.Offset, dataSize));

                buffer = new ArraySegment<byte>(buffer.Array, buffer.Offset + dataSize, buffer.Count - dataSize);
                processLength += dataSize;
            }

            return processLength;
        }

        public void Send(IMessage packet)
        {
            ushort size = (ushort)packet.CalculateSize();


            string msgName = packet.Descriptor.Name.Replace("_", string.Empty);
            MsgID msgID = (MsgID)Enum.Parse(typeof(MsgID), msgName);

            ushort id = (ushort)msgID;
            byte[] sendBuffer = new byte[size + 4];
            
            Array.Copy(BitConverter.GetBytes((ushort)(size + 4)), 0, sendBuffer, 0, sizeof(ushort));
            Array.Copy(BitConverter.GetBytes(id), 0, sendBuffer, 2, sizeof(ushort));
            Array.Copy(packet.ToByteArray(), 0, sendBuffer, 4, size);
   
            Send(new ArraySegment<byte>(sendBuffer));
        }

        public abstract void OnReceivePacket(ArraySegment<byte> buffer);
    }
}
