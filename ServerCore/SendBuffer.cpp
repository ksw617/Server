#include "pch.h"
#include "SendBuffer.h"

SendBuffer::SendBuffer(int size)
{
    buffer.resize(size);
}

SendBuffer::~SendBuffer()
{
}

bool SendBuffer::CopyData(void* data, int len)
{
	if (Capacity() < len)
		return false;

	memcpy(buffer.data(), data, len);
	writeSize = len;
    return true;
}
