#pragma once
#include "IocpObj.h"
class Session : public IocpObj
{
private:
	SOCKET socket = INVALID_SOCKET;
	SOCKADDR_IN sockAddr = {};
public:
	char recvBuffer[1024] = {};
public:
	Session();
	virtual ~Session();
	
public:
	SOCKET GetSocket() { return socket; }
public:
	//sockAddr는 클라의 주소 담고 있음
	void SetSockAddr(SOCKADDR_IN address) { sockAddr = address; }
public:
	void ProcessConnect();
private:
	// Inherited via IocpObj
	HANDLE GetHandle() override;
	void ObserveIO(IocpEvent* iocpEvent, int numOfBytes) override;
};

