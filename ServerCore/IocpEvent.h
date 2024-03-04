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


class AcceptEvent : public IocpEvent
{
public:
	Session* session = nullptr;
public:
	//������ ȣ�� �ɶ� �θ� �ִ� ������ IocpEvent(EventType type) ȣ�� �Ǵµ�
	//�Ű������� EventType::ACCEPT�� �־���
	AcceptEvent() : IocpEvent(EventType::ACCEPT) {}
};
