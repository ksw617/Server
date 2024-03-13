#include "pch.h"
#include "Session.h"
#include "SocketHelper.h"
#include "Service.h"

Session::Session()
{
	socket = SocketHelper::CreateSocket();
}

Session::~Session()
{
	SocketHelper::CloseSocket(socket);
}


void Session::ProcessConnect()
{
	//connected = true;
	connected.store(true);

	GetService()->AddSession(this);

	OnConnected();

	RegisterRecv();
	
}

void Session::RegisterRecv()
{
	if (!IsConnected())
		return;

	//하나 만들어서 재활용
	recvEvent.Init();
	recvEvent.iocpObj = this;

	WSABUF wsaBuf;
	wsaBuf.buf = (char*)recvBuffer;
	wsaBuf.len = sizeof(recvBuffer);

	DWORD recvLen = 0;
	DWORD flags = 0;

	if (WSARecv(socket, &wsaBuf, 1, &recvLen, &flags, &recvEvent, nullptr) == SOCKET_ERROR) 
	{
		int errorCode = WSAGetLastError();
		if (errorCode != WSA_IO_PENDING)
		{
			HandleError(errorCode);
			recvEvent.iocpObj = nullptr;
		}
	}
}
HANDLE Session::GetHandle()
{
	return (HANDLE)socket;
}

void Session::ObserveIO(IocpEvent* iocpEvent, int numOfBytes)
{
	switch (iocpEvent->eventType)
	{
	case EventType::RECV:
		ProcessRecv(numOfBytes);
		break;
	default:
		break;
	}
}


void Session::ProcessRecv(int numOfBytes)
{
	recvEvent.iocpObj = nullptr;
	printf("Recv Data : %d\n", numOfBytes);
	RegisterRecv();
}

void Session::HandleError(int errorCode)
{
	switch (errorCode)
	{
	case WSAECONNRESET:
	case WSAECONNABORTED:
		printf("Handle Error\n");
		break;
	default:
		break;
	}
}

