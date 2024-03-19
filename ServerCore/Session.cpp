#include "pch.h"
#include "Session.h"
#include "SocketHelper.h"
#include "Service.h"

Session::Session() : recvBuffer(BUFFER_SIZE) // 생성자 호출되면서 초기화
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


bool Session::Connect()
{
	return RegisterConnect();
}

bool Session::RegisterConnect()
{
	if (IsConnected())
		return false;
	if (GetService()->GetServiceType() != ServiceType::CLIENT)
		return false;
	if (!SocketHelper::SetReuseAddress(socket, true))
		return false;
	if (SocketHelper::BindAnyAddress(socket, 0) == false)
		return false;

	connectEvent.Init();
	connectEvent.iocpObj = shared_from_this();

	DWORD numOfBytes = 0;
	SOCKADDR_IN sockAddr = GetService()->GetSockAddr();
	if (SocketHelper::ConnectEx(socket, (SOCKADDR*)&sockAddr, sizeof(sockAddr), nullptr, 0, &numOfBytes, &connectEvent))
	{
		int errorCode = WSAGetLastError();
		if (errorCode != ERROR_IO_PENDING)
		{
			HandleError(errorCode);
			connectEvent.iocpObj = nullptr;
			return false;

		}
	}

	return true;
}

void Session::ProcessConnect()
{
	connectEvent.iocpObj = nullptr;

	connected.store(true);
	GetService()->AddSession(GetSession());

	OnConnected();

	RegisterRecv();
	
}

void Session::RegisterRecv()
{
	if (!IsConnected())
		return;

	recvEvent.Init();
	recvEvent.iocpObj = shared_from_this();

	WSABUF wsaBuf;
	wsaBuf.buf = (char*)recvBuffer.WritePos();
	wsaBuf.len = recvBuffer.FreeSize();

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

void Session::ObserveIO(IocpEvent* iocpEvent, int numOfBytes)
{
	switch (iocpEvent->eventType)
	{
	case EventType::CONNECT:
		ProcessConnect();
		break;
	case EventType::RECV:
		ProcessRecv(numOfBytes);
		break;
	case EventType::SEND:
		ProcessSend(numOfBytes);
		break;
	case EventType::DISCONNECT:
		ProcessDisconnect();
		break;
	default:
		break;
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

	if (recvBuffer.OnWrite(numOfBytes) == false)
	{
		Disconnect(L"On Write overflow");
		return;
	}

	int dataSize = recvBuffer.DataSize();

	int processLen = OnRecv(recvBuffer.ReadPos(), numOfBytes);
	if (processLen < 0 || dataSize < processLen || recvBuffer.OnRead(processLen) == false)
	{
		Disconnect(L"On Read overflow");
		return;
	}

	recvBuffer.Clear();

	RegisterRecv();
}



//수정
void Session::Send(shared_ptr<SendBuffer> sendBuffer)
{
	unique_lock<shared_mutex> lock(rwLock);

	sendQueue.push(sendBuffer);
	if (sendReistered.exchange(true) == false)
	{
		RegisterSend();
	}

}

void Session::RegisterSend()
{

	if (IsConnected() == false)
		return;

	WSABUF wsaBuf;
	//wsaBuf.buf = (char*)sendEvent->sendBuffer.data();
	//wsaBuf.len = (ULONG)sendEvent->sendBuffer.size();
	//
	//DWORD numOfBytes = 0;
	//if (WSASend(socket, &wsaBuf, 1, &numOfBytes, 0, sendEvent, nullptr) == SOCKET_ERROR)
	//{
	//	int errorCode = WSAGetLastError();
	//	if (errorCode != WSA_IO_PENDING)
	//	{
	//		HandleError(errorCode);
	//		sendEvent->iocpObj = nullptr;
	//		delete sendEvent;
	//
	//	}
	//
	//}
}




void Session::ProcessSend(int numOfBytes)
{
	//sendEvent->iocpObj = nullptr;
	//delete sendEvent;
	//
	//if (numOfBytes == 0)
	//{
	//	Disconnect(L"Send 0 bytes");
	//
	//}

	OnSend(numOfBytes);
}




void Session::Disconnect(const WCHAR* cause)
{
	if (connected.exchange(false) == false)
		return;

	wprintf(L"disconnect reason : %ls\n", cause);

	OnDisconnected();

	GetService()->RemoveSession(GetSession());

	RegisterDisConnect();
}

bool Session::RegisterDisConnect()
{
	disConnectEvent.Init();
	disConnectEvent.iocpObj = shared_from_this();

	if (SocketHelper::DisconnectEx(socket, &disConnectEvent, TF_REUSE_SOCKET, 0))
	{
		int errorCode = WSAGetLastError();
		if (errorCode != WSA_IO_PENDING)
		{
			HandleError(errorCode);
			disConnectEvent.iocpObj = nullptr;
			return false;

		}
	}
	return true;
}



void Session::ProcessDisconnect()
{
	disConnectEvent.iocpObj = nullptr;
}



void Session::HandleError(int errorCode)
{
	switch (errorCode)
	{
	case WSAECONNRESET:
	case WSAECONNABORTED:
		Disconnect(L"Handle Error");
		break;
	default:
		printf("ErrorCode : %d\n", errorCode);
		break;
	}
}