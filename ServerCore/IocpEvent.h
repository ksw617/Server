#pragma once

enum class EventType : u_char
{ 
	CONNECT,
	DISCONNECT,
	ACCEPT,
	RECV,
	SEND,
};

class Session;

class IocpEvent	: public OVERLAPPED
{
public:
	EventType eventType;
	shared_ptr<class IocpObj> iocpObj;
public:
	IocpEvent(EventType type);
public:
	void Init();
};


class ConnectEvent : public IocpEvent
{
public:
	ConnectEvent() : IocpEvent(EventType::CONNECT) {}
};

class AcceptEvent : public IocpEvent
{
public:
	shared_ptr<Session> session = nullptr;
public:
	AcceptEvent() : IocpEvent(EventType::ACCEPT) {}
};

class RecvEvent : public IocpEvent
{
public:
	RecvEvent() : IocpEvent(EventType::RECV) {}

};


class SendEvent : public IocpEvent
{
public:
	//sendBuffer class º¯È¯
	//vector<SendBuffer*>
	vector<shared_ptr<class SendBuffer>> sendBuffers;
	//vector<BYTE> sendBuffer;
public:
	SendEvent() : IocpEvent(EventType::SEND) {}

};


class DisConnectEvent : public IocpEvent
{
public:
	DisConnectEvent() : IocpEvent(EventType::DISCONNECT) {}
};