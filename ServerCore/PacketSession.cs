using System;

namespace ServerCore
{
    public abstract class PacketSession : Session
    {
        //[size[2]][ID[2]][][][][][][][][][][]....
        public sealed override int OnReceive(ArraySegment<byte> buffer) // 10
        {
            //얼마나 처리했는지 반환하기 위해
            int processLength = 0;

            while (true)
            {
                //최소한 사이즈받는 2byte보다는 커야 하니까         
                if (buffer.Count < 2)
                {
                    //다음 받을 때까지 기다림
                    break;
                }

                //최소한 거기서 보낸것이 2byte이상임

                //BitConverter : byte -> 원하는 자료형
                //BitConverter.ToUInt16 : byte -> ushort 변환
                //BitConverter.ToUInt16(배열, 시작) : ToUInt16 얘가 2byte 변환이니까
                //{[][]}[][][][][]... 앞의 2byte만 변환해서 
                ushort dataSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
                //보낸 배열의 크기값이 내가 받아야 할 사이즈 보다 작다면.
                if (buffer.Count < dataSize)
                {
                    //다음 받을 때까지 기다림
                    break;
                }

                //여기까지 왔다면 패킷 조립 가능

                //우선 조립해서 보내고
                OnReceivePacket(new ArraySegment<byte>(buffer.Array, buffer.Offset, dataSize));

                //([][][][][][]){[][][][][][]}[][][] () 조립이 되었구 {} 또 조립시키기 위해
                //많이 받아서 다음꺼도 조립을 시키기 위해
                //()이후부분을 버퍼에 넣어줌
                //buffer.Offset + dataSize : { 여기서 부터
                //buffer.Count - dataSize : 전체 배열에서 아까 조립한 부분만큼 뺀 크기
                buffer = new ArraySegment<byte>(buffer.Array, buffer.Offset + dataSize, buffer.Count - dataSize);

                //dataSize만큼 처리했다고 더해줌
                processLength += dataSize;
            }

            //처리 끝났으면 얼마나 처리했다고 반환
            return processLength;
        }

        public abstract void OnReceivePacket(ArraySegment<byte> buffer);
    }
}
