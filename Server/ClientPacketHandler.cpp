#include "pch.h"
#include "ClientPacketHandler.h"

bool Handle_INVALID(shared_ptr<PacketSession>& session, BYTE* buffer, int len)
{
    return false;
}

bool Handle_C_LOGIN(shared_ptr<PacketSession>& session, Protocol::C_LOGIN& packet)
{
    return false;
}

void ClientPacketHandler::Init()
{
}

bool ClientPacketHandler::HandlePacket(shared_ptr<PacketSession>& session, BYTE* buffer, int len)
{
    return false;
}
