#include "pch.h"
#include "RecvBuffer.h"

RecvBuffer::RecvBuffer(int size)  : bufferSize(size)
{
    //엄청나게 큰 공간 할당
    capacity = bufferSize * BUFFER_COUNT;
    buffer.resize(bufferSize); 
}

RecvBuffer::~RecvBuffer()
{
}

void RecvBuffer::Clear()
{           
    //만날 확률을 높이는 방식
    int dataSize = DataSize();  
    if (dataSize == 0)         
    {
        readPos = 0;           
        writePos = 0;          
    }
    else
    {
        //[   ][   ][   ][   ][   ][   ][   ][   ][   ][r w ] 
        if (FreeSize() < bufferSize)
        {
            //복사 진행
            memcpy(&buffer[0], &buffer[readPos], dataSize);
            readPos = 0;
            writePos = dataSize;
        }
      
    }                        
}

bool RecvBuffer::OnRead(int numOfByte)
{
    if (numOfByte > DataSize())
        return false;

    readPos += numOfByte;

    return true;
}

bool RecvBuffer::OnWrite(int numOfByte)
{
    if (numOfByte > FreeSize())
        return false;

    writePos += numOfByte;

    return true;
}
