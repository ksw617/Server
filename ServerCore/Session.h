#pragma once
#include "IocpObj.h"

class Service;

class Session : public IocpObj
{
	friend class Listener;

private:
	shared_mutex rwLock;
	atomic<bool> connected = false;
	Service* service = nullptr;
	SOCKET socket = INVALID_SOCKET;
	SOCKADDR_IN sockAddr = {};
private:
	//Connect 이벤트 추가
	ConnectEvent connectEvent;
	RecvEvent recvEvent;
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
	Service* GetService() const { return service; }
public:
	void SetService(Service* _service) { service = _service; }
	void SetSockAddr(SOCKADDR_IN address) { sockAddr = address; }
private:
	//Connect 등록
	bool RegisterConnect();
	void RegisterRecv();
	void RegisterSend(SendEvent* sendEvent);
private:
	void ProcessConnect();
	void ProcessRecv(int numOfBytes);
	void ProcessSend(SendEvent* sendEvent, int numOfBytes);
private:
	void HandleError(int errorCode);
protected:
	virtual void OnConnected() {}
	virtual int OnRecv(BYTE* buffer, int len) { return len; }
	virtual void OnSend(int len) {}
	virtual void OnDisconnected() {}
public:
	//연결 할 아이
	bool Connect();
	void Send(BYTE* buffer, int len);
	void Disconnect(const WCHAR* cause);

};

