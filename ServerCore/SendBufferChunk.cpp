#include "pch.h"
#include "SendBufferChunk.h"
#include "SendBuffer.h"

void SendBufferChunk::Init()
{
	open = false;
	usedSize = 0;
}

shared_ptr<SendBuffer> SendBufferChunk::Open(int size)
{
	if (size > SEND_BUFFER_SIZE)
		return nullptr;
	if (open)
		return nullptr;

	if (size > FreeSize())
		return nullptr;

	open = true;

	return make_shared<SendBuffer>(shared_from_this(), &buffer[usedSize], size);
}

void SendBufferChunk::Close(int size)
{
	//열려있지 않으면 안되니까
	if (!open)
		return;

	//닫아주고
	open = false;
	//얼마나 사용했는지 추가
	usedSize += size;
}
