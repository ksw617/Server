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
	class IocpObj* iocpObj;
public:
	IocpEvent(EventType type);
public:
	void Init();
};

//Connect �߰�
class ConnectEvent : public IocpEvent
{
public:
	ConnectEvent() : IocpEvent(EventType::CONNECT) {}
};

class AcceptEvent : public IocpEvent
{
public:
	Session* session = nullptr;
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
	vector<BYTE> sendBuffer;
public:
	SendEvent() : IocpEvent(EventType::SEND) {}

};