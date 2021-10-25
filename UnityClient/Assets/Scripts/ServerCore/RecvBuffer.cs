using System;

namespace ServerCore
{
    class RecvBuffer
    {
        //TCP 
        //4byte
        //6byte
        //[][][][][r][][1][w][1][1][][][][][][][][]....
        //byte 배열
        ArraySegment<byte> buffer;
        //처리한 애들<- readPos <- 처리해야 하는 애들 -> writePos -> 사용할수 있는 공간
        int readPos; 
        int writePos;

        //생성할때 사이즈 크기 할당
        public RecvBuffer(int bufferSize)
        {
            //버퍼 크기 할당
            buffer = new ArraySegment<byte>(new byte[bufferSize], 0, bufferSize);
        }

        //w == 7, r == 4  w - r == 3
        //[][][][]{[r][][]}[w][][][][][][][][][][].... {} <- 요부분
        //사용한 크기
        public int DataSize { get => writePos - readPos; }

        //buffer.Count == byte배열의 크기값
        //[][][][][r][][]{[w][][][][][][][][][][]....} {} <- 요부분
        //남은 크기
        public int FreeSize { get => buffer.Count - writePos; }

        //buffer.Offset == 0 + r == 4  DataSize == 3 r~> 3칸
        //[][][][]{[r][][]}[w][][][][][][][][][][].... {} <- 요부분
        public ArraySegment<byte> DataSegment
        {  
            //                           (배열         , 시작점                  , 갯수   );   
            get => new ArraySegment<byte>(buffer.Array, buffer.Offset + readPos, DataSize);
        }

        //buffer.Offset == 0 + w == 7  FreeSize == 3 w~> 전체크기에서 - w == FreeSize
        //[][][][][r][][]{[w][][][][][][][][][][]....} {} <- 요부분
        public ArraySegment<byte> FreeSegment
        {
            //                           (배열         , 시작점                  , 갯수   );   
            get => new ArraySegment<byte>(buffer.Array, buffer.Offset + writePos, FreeSize);
        }


        //[][][][][r][][][w][][][][][][][][][][]....
        public void Clean()
        {
            //[][][][][][][][rw][][][][][][][][][][].... rw위치가 같다면 처음으로
            if (DataSize == 0)
            {
                readPos = 0;
                writePos = 0;
            }
            else  //[][][][][r][][][w][][][][][][][][][][].... rw위치가 다르다면
            {
                //[][][][]{[r][][]}[w][][][][][][][][][][].... 
                //{[][][]}[r][][][][w][][][][][][][][][][].... 
                //Array.Copy(원본 배열, 원본배열의 시작점, 만들 배열, 만들 배열의 시작점, 크기)
                //우선 복사함
                Array.Copy(buffer.Array, buffer.Offset + readPos, buffer.Array, buffer.Offset, DataSize);

                //[r][][][][][][][w][][][][][][][][][][].... 
                readPos = 0;
                //[r][][][w][][][][][][][][][][][][][][].... 
                writePos = DataSize;
            }
        }

        //[r][][][][][][][w][][][][][][][][][][]....
        //[][][][][r][][][w][][][][][][][][][][]....
        public bool OnRead(int numberOfBytes)
        {
            //만약에 읽은 데이터가 읽어야 하는 데이터 보다 크다면
            if (numberOfBytes > DataSize)
            {
                //false 반환
                return false;
            }

            //readPos의 위치값 numberOfBytes 만큼 이동 
            readPos += numberOfBytes;
            //제대로 읽었다 true 반환
            return true;
        }


        //[r][][w][][][][][][][][][][][][][][][]....
        //[r][][][][][][][w][][][][][][][][][][]....
        public bool OnWrite(int numberOfBytes)
        {
            //쓴데이터가 남은 데이터보다 크다면
            if (numberOfBytes > FreeSize)
            {
                //false반환
                return false;
            }
            //writePos 위치값 numberOfBytes 만큼 이동 
            writePos += numberOfBytes;
            //제대로 읽었다 true 반환
            return true;    
        }
    }
}
