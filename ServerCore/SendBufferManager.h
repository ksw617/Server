#pragma once
#include "SendBuffer.h"

class SendBufferChunk : public enable_shared_from_this<SendBufferChunk>
{
	enum {SEND_BUFFER_SIZE = 0x10000};
private:
	vector<BYTE> buffer;
public:
	SendBufferChunk() : buffer(SEND_BUFFER_SIZE) {}
	~SendBufferChunk() {}
};



class SendBufferManager
{
private:
	shared_mutex rwLock;
	vector<shared_ptr<SendBufferChunk>> sendBufferChunks;
public:
	shared_ptr<SendBuffer> Open(int size);
public:
	shared_ptr<SendBufferChunk> Pop();
	void Push(shared_ptr<SendBufferChunk> bufferChunk);
	static void PushGlobal(SendBufferChunk* bufferChunck);
};

