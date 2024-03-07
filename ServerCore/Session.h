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
	//sockAddr�� Ŭ���� �ּ� ��� ����
	void SetSockAddr(SOCKADDR_IN address) { sockAddr = address; }
public:
	void ProcessConnect();
private:
	// Inherited via IocpObj
	HANDLE GetHandle() override;
	void ObserveIO(IocpEvent* iocpEvent, int numOfBytes) override;
};
