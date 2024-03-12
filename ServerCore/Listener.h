#pragma once
#include "IocpObj.h"

class ServerService;
class AcceptEvent;

class Listener : public IocpObj
{
private:
	ServerService* serverService = nullptr;
	SOCKET socket = INVALID_SOCKET;
public:
	Listener() {}
	//virtual ¼Ò¸êÀÚ
	virtual ~Listener();
public:
	bool StartAccept(ServerService* service);
	void CloseSocket();
public:
	void RegisterAccept(AcceptEvent* acceptEvent);
	void ProcessAccept(AcceptEvent* acceptEvent);
public:
	HANDLE GetHandle() override;
	void ObserveIO(class IocpEvent* iocpEvent, int numOfBytes) override;
};

