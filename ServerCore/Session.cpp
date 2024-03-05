#include "pch.h"
#include "Session.h"
#include "SocketHelper.h"

Session::Session()
{
	//소켓 하나 만들어줌
	//Server에서는 AcceptSocket
	socket = SocketHelper::CreateSocket();
}

Session::~Session()
{
	SocketHelper::CloseSocket(socket);
}

void Session::ProcessConnect()
{
	//Todo
	printf("Seesion::ProcessConnect\n");
}

HANDLE Session::GetHandle()
{
	return (HANDLE)socket;
}

void Session::ObserveIO(IocpEvent* iocpEvent, int numOfBytes)
{
	//Todo
}
