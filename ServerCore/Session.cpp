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

HANDLE Session::GetHandle()
{
	return (HANDLE)socket;
}


void Session::ProcessConnect()
{
	connected.store(true);

	GetService()->AddSession(this);

	OnConnected();

	RegisterRecv();
	
}


void Session::RegisterRecv()
{
	if (!IsConnected())
		return;

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

void Session::ProcessRecv(int numOfBytes)
{
	recvEvent.iocpObj = nullptr;

	if (numOfBytes == 0)
	{
		Disconnect(L"Recv 0 bytes");
		return;
	}

	OnRecv(recvBuffer, numOfBytes);
	RegisterRecv();
}




void Session::Send(BYTE* buffer, int len)
{
	//SendEvent 만들기
	SendEvent* sendEvent = new SendEvent();
	//sendEvent iocpObj는 여기
	sendEvent->iocpObj = this;
	//sendEvent의 vector<Byte> sendBuffer <- 크기값을 len로
	sendEvent->sendBuffer.resize(len);
	//buffer의 데이터를 복사해서 sendEvent->sendBuffer에다가 붙여넣기
	memcpy(sendEvent->sendBuffer.data(), buffer, len);

	unique_lock<shared_mutex> lock(rwLock);
	RegisterSend(sendEvent);

}

void Session::RegisterSend(SendEvent* sendEvent)
{

	//연결 상태가 아니라면
	if (IsConnected() == false)
		return;

	//WSASend 구현
	WSABUF wsaBuf;
	wsaBuf.buf = (char*)sendEvent->sendBuffer.data();
	wsaBuf.len = (ULONG)sendEvent->sendBuffer.size();

	DWORD numOfBytes = 0;
	//SendEvent 등록
	if (WSASend(socket, &wsaBuf, 1, &numOfBytes, 0, sendEvent, nullptr) == SOCKET_ERROR)
	{
		int errorCode = WSAGetLastError();
		//에러 발생시
		if (errorCode != WSA_IO_PENDING)
		{
			HandleError(errorCode);
			sendEvent->iocpObj = nullptr;
			delete sendEvent;

		}

	}
}

void Session::ObserveIO(IocpEvent* iocpEvent, int numOfBytes)
{
	switch (iocpEvent->eventType)
	{
	case EventType::RECV:
		ProcessRecv(numOfBytes);
		break;
	case EventType::SEND:	//이벤트 타입이 Send 였을경우
		ProcessSend((SendEvent*)iocpEvent, numOfBytes);
		break;
	default:
		break;
	}
}



void Session::ProcessSend(SendEvent* sendEvent, int numOfBytes)
{
	sendEvent->iocpObj = nullptr;
	delete sendEvent;

	if (numOfBytes == 0)
	{
		Disconnect(L"Send 0 bytes");

	}

	OnSend(numOfBytes);
}



void Session::Disconnect(const WCHAR* cause)
{
	if (connected.exchange(false) == false)
		return;

	wprintf(L"disconnect reason : %ls\n", cause);

	OnDisconnected();
	SocketHelper::CloseSocket(socket);
	GetService()->RemoveSession(this);
	
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