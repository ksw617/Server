#pragma once
#include <PacketSession.h>
#include "Protocol.pb.h"

enum :uint16
{
	C_LOGIN = 0,
	S_LOGIN = 1,
};


class ServerPacketHandler
{
public:
	static void HandlePacket(BYTE* buffer, int len);
private:
	static void Handle_S_LOGIN(BYTE* buffer, int len);
};

