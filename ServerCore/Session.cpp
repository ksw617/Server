#include "pch.h"
#include "Session.h"
#include "SocketHelper.h"

Session::Session()
{
	//���� �ϳ� �������
	//Server������ AcceptSocket
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
