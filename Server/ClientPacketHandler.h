#pragma once
#include <PacketSession.h>
#include <SendBufferManager.h>
#include <functional>
#include "Protocol.pb.h"

enum :uint16
{
	C_LOGIN = 0,
	S_LOGIN = 1,
};

bool Handle_INVALID(shared_ptr<PacketSession>& session, BYTE* buffer, int len);
bool Handle_C_LOGIN(shared_ptr<PacketSession>& session, Protocol::C_LOGIN& packet);


class ClientPacketHandler
{
public:
	using PacketHeaderFunc = function<bool(shared_ptr<PacketSession>&, BYTE*, int)>;
	static PacketHeaderFunc packetHandlers[UINT16_MAX];

public:
	//Recv
	static void Init();
	static bool HandlePacket(shared_ptr<PacketSession>& session, BYTE* buffer, int len);
public:
	//Send
	static shared_ptr<SendBuffer> MakeSendBuffer(Protocol::S_LOGIN& packet) { return MakeSendBuffer(packet, S_LOGIN); }
private:
	template<typename PacketType, typename ProcessFunc>
	static bool HandlePacket(ProcessFunc func, shared_ptr<PacketSession>& session, BYTE* buffer, int len)
	{
		PacketType packet;
		packet.ParseFromArray(buffer + sizeof(PacketHeader), len - sizeof(PacketHeader));

		return func(session, packet);
	}


	template<typename T>
	static shared_ptr<SendBuffer> MakeSendBuffer(T& packet, uint16 id)
	{
		uint16 dataSize = (uint16)packet.ByteSizeLong();
		uint16 packetSize = dataSize + sizeof(PacketHeader);

		shared_ptr<SendBuffer> sendBuffer = SendBufferManager::Get().Open(4096);
		PacketHeader* buffer = (PacketHeader*)sendBuffer->GetBuffer();
		buffer->size = packetSize;
		buffer->id = id; 

		if (!packet.SerializeToArray(&buffer[1], dataSize))
		{
			sendBuffer->Close(0);
			return nullptr;
		}

		if (!sendBuffer->Close(packetSize))
		{
			return nullptr;
		}

		return sendBuffer;
	}
};

