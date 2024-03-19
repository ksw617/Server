#include "pch.h"
#include "RecvBuffer.h"

RecvBuffer::RecvBuffer(int size)  : bufferSize(size)
{
    //��û���� ū ���� �Ҵ�
    capacity = bufferSize * BUFFER_COUNT;
    buffer.resize(bufferSize); 
}

RecvBuffer::~RecvBuffer()
{
}

void RecvBuffer::Clear()
{           
    //���� Ȯ���� ���̴� ���
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
            //���� ����
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
