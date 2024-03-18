#pragma once
#include "IocpObj.h"

class ServerService;
class AcceptEvent;

class Listener : public IocpObj
{
private:
	//스마트 포인터로 변환
	shared_ptr<ServerService> serverService = nullptr;
	SOCKET socket = INVALID_SOCKET;
public:
	Listener() {}
	virtual ~Listener();
public:
	//스마트 포인터로 변환
	bool StartAccept(shared_ptr<ServerService> service);
	void CloseSocket();
public:
	void RegisterAccept(AcceptEvent* acceptEvent);
	void ProcessAccept(AcceptEvent* acceptEvent);
public:
	HANDLE GetHandle() override;
	void ObserveIO(class IocpEvent* iocpEvent, int numOfBytes) override;
};

