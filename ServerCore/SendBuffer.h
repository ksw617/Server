#pragma once
class SendBufferChunk;

class SendBuffer : public enable_shared_from_this<SendBuffer>
{
private:
	BYTE* buffer;
	int freeSize = 0;
	int writeSize = 0;
	shared_ptr<SendBufferChunk> sendBufferChunk;

public:
	SendBuffer(shared_ptr<class SendBufferChunk> chunk, BYTE* start, int size);
	~SendBuffer();
public:
	BYTE* GetBuffer() { return buffer; }
	int WriteSize() const { return writeSize; }
public:
	//내가 쓴만큼 넣어주기
	bool Close(int usedSize);
};

