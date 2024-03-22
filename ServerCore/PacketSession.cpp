#include "pch.h"
#include "PacketSession.h"


//[0]                  [13][14]                 [27]
//[packet                 ][packet                 ]...
//[size(2)][id(2)][data...][size(2)][id(2)][data...]...
int PacketSession::OnRecv(BYTE* buffer, int len)
{
    int processLen = 0;
    while (true)
    {  
        int dataSize = len - processLen;

        if (dataSize < sizeof(PacketHeader))
            break; 

      
        PacketHeader header = *(PacketHeader*)(&buffer[processLen]);

        if (dataSize < header.size)
            break;

        OnRecvPacket(&buffer[processLen], header.size);

        processLen += header.size;
    }

    return processLen;
}
