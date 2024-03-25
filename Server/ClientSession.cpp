#include "pch.h"
#include "ClientSession.h"
#include "SessionManager.h"	  
#include <SendBufferManager.h>	


void ClientSession::OnConnected()
{
	SessionManager::Get().Add(static_pointer_cast<ClientSession>(shared_from_this()));
}
int ClientSession::OnRecvPacket(BYTE* buffer, int len)
{
	shared_ptr<SendBuffer> sendBuffer = SendBufferManager::Get().Open(4096);
	memcpy(sendBuffer->GetBuffer(), buffer, len);

	if (sendBuffer->Close(len))
	{
		Send(sendBuffer);
	}

	printf("%s\n", &buffer[4]);
	return len;
}
void ClientSession::OnSend(int len)
{
}
void ClientSession::OnDisconnected()
{
	SessionManager::Get().Remove(static_pointer_cast<ClientSession>(shared_from_this()));
}
