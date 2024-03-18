#pragma once
#include "IocpObj.h"

class Service;

class Session : public IocpObj
{
	friend class Listener;

private:
	shared_mutex rwLock;
	atomic<bool> connected = false;
	shared_ptr<Service> service = nullptr;
	SOCKET socket = INVALID_SOCKET;
	SOCKADDR_IN sockAddr = {};
private:
	ConnectEvent connectEvent;
	RecvEvent recvEvent;
	DisConnectEvent disConnectEvent;
public:
	BYTE recvBuffer[1024] = {};	 
public:
	Session();
	virtual ~Session();
private:
	HANDLE GetHandle() override;
	void ObserveIO(class IocpEvent* iocpEvent, int numOfBytes) override;
public:
	SOCKET GetSocket() const { return socket; }
	bool IsConnected() const { return connected; }

	shared_ptr<Service> GetService() const { return service; }
	shared_ptr<Session> GetSession() {  return static_pointer_cast<Session>(shared_from_this()); }
public:

	void SetService(shared_ptr<Service> _service) { service = _service; }
	void SetSockAddr(SOCKADDR_IN address) { sockAddr = address; }
private:
	bool RegisterConnect();
	void RegisterRecv();
	void RegisterSend(SendEvent* sendEvent);
	bool RegisterDisConnect();
private:
	void ProcessConnect();
	void ProcessRecv(int numOfBytes);
	void ProcessSend(SendEvent* sendEvent, int numOfBytes);
	void ProcessDisconnect();
private:
	void HandleError(int errorCode);
protected:
	virtual void OnConnected() {}
	virtual int OnRecv(BYTE* buffer, int len) { return len; }
	virtual void OnSend(int len) {}
	virtual void OnDisconnected() {}
public:
	bool Connect();
	void Send(BYTE* buffer, int len);
	void Disconnect(const WCHAR* cause);

};

