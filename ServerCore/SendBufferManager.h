#pragma once
#include "SendBuffer.h"
#include "SendBufferChunk.h"

class SendBufferManager
{
private:
	SendBufferManager() = default;
	~SendBufferManager() = default;
public:
	static SendBufferManager& Get()
	{
		static SendBufferManager instance;
		return instance;
	}
public:
	SendBufferManager(const SendBufferManager&) = delete;
	SendBufferManager& operator= (const SendBufferManager&) = delete;

private:
	shared_mutex rwLock;
	vector<shared_ptr<SendBufferChunk>> sendBufferChunks;
public:
	static thread_local shared_ptr<SendBufferChunk> localSendBufferChunk;
public:
	shared_ptr<SendBuffer> Open(int size);
public:
	shared_ptr<SendBufferChunk> Pop();
	void Push(shared_ptr<SendBufferChunk> bufferChunk);
	static void PushGlobal(SendBufferChunk* bufferChunck);
};