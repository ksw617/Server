#include "pch.h"
#include "ServerPacketHandler.h"

void ServerPacketHandler::HandlePacket(BYTE* buffer, int len)
{
	PacketHeader* header = (PacketHeader*)buffer;
	switch (header->id)
	{
	case S_LOGIN:
		Handle_S_LOGIN(buffer, len);
		break;
	default:
		break;
	}
}

void ServerPacketHandler::Handle_S_LOGIN(BYTE* buffer, int len)
{
	//Protocol::Login packet;
	//packet.ParseFromArray(buffer + sizeof(PacketHeader), len - sizeof(PacketHeader));
	//
	//if (packet.has_player())
	//{
	//	const Protocol::Player& player = packet.player();
	//	printf("Player id : %d, player name : %s\n", player.id(), player.name().c_str());
	//}
}
