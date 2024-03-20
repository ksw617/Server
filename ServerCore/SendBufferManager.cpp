#include "pch.h"
#include "SendBufferManager.h"

shared_ptr<SendBuffer> SendBufferManager::Open(int size)
{
	return shared_ptr<SendBuffer>();
}

shared_ptr<SendBufferChunk> SendBufferManager::Pop()
{
	return shared_ptr<SendBufferChunk>();
}

void SendBufferManager::Push(shared_ptr<SendBufferChunk> bufferChunk)
{
}

void SendBufferManager::PushGlobal(SendBufferChunk* bufferChunck)
{
}
