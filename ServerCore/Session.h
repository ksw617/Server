#pragma once
#include "IocpObj.h"
#include "RecvBuffer.h"
#include "SendBuffer.h"

class Service;

class Session : public IocpObj
{
	friend class Listener;
	enum {BUFFER_SIZE = 0x10000}; 

private:
	shared_mutex rwLock;
	atomic<bool> connected = false;
	shared_ptr<Service> service = nullptr;
	SOCKET socket = INVALID_SOCKET;
	SOCKADDR_IN sockAddr = {};
private:
	ConnectEvent connectEvent;
	RecvEvent recvEvent;
	//SendEvent 추가
	SendEvent sendEvent;
	DisConnectEvent disConnectEvent;
private:
	RecvBuffer recvBuffer;
	//sendBuffer queue 추가
	queue<shared_ptr<SendBuffer>> sendQueue;
	//atomic으로 내가 보냈는지 안보냈는지
	atomic<bool> sendReistered = false;
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
	//SendEvent 들구 있을꺼라
	void RegisterSend();
	bool RegisterDisConnect();
private:
	void ProcessConnect();
	void ProcessRecv(int numOfBytes);
	//SendEvent 들구 있을꺼라
	void ProcessSend(int numOfBytes);
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
	//Send 수정
	void Send(shared_ptr<SendBuffer> sendBuffer);
	void Disconnect(const WCHAR* cause);

};

