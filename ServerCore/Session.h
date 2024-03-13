#pragma once
#include "IocpObj.h"

class Service;

class Session : public IocpObj
{
	friend class Listener;

private:
	atomic<bool> connected = false;
	Service* service = nullptr;
	SOCKET socket = INVALID_SOCKET;
	SOCKADDR_IN sockAddr = {};
private:
	RecvEvent recvEvent;
public:
	char recvBuffer[1024] = {};
public:
	Session();
	virtual ~Session();
private:
	// Inherited via IocpObj
	HANDLE GetHandle() override;
	void ObserveIO(class IocpEvent* iocpEvent, int numOfBytes) override;
public:
	//Get
	SOCKET GetSocket() const { return socket; }
	bool IsConnected() const { return connected; }
	Service* GetService() const { return service; }
public:
	//Set
	void SetService(Service* _service) { service = _service; }
	void SetSockAddr(SOCKADDR_IN address) { sockAddr = address; }
private:
	void RegisterRecv();
private:
	void ProcessConnect();
	void ProcessRecv(int numOfBytes);
private:
	void HandleError(int errorCode);
protected:
	//외부에서 상속받아서 사용할 용도로 -> ServierSession
	virtual void OnConnected() {}
	virtual int OnRecv(BYTE* buffer, int len) { return len; }


};

