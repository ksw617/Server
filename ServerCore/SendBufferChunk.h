#pragma once
class SendBufferChunk : public enable_shared_from_this<SendBufferChunk>
{
	enum { SEND_BUFFER_SIZE = 0x10000 };
private:
	vector<BYTE> buffer;
	bool open = false;
	int usedSize = 0;
public:
	SendBufferChunk() : buffer(SEND_BUFFER_SIZE) {}
	~SendBufferChunk() {}
public:
	void Init();
	shared_ptr<class SendBuffer> Open(int size);
	//다쓰고 얼마나 썻는지.
	void Close(int size);
public:
	bool IsOpen() const { return open; }
	int FreeSize() const { return buffer.size() - usedSize;  }
};
