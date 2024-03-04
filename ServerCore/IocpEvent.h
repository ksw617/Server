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
	//생성자 호출 될때 부모에 있는 생성자 IocpEvent(EventType type) 호출 되는데
	//매개변수로 EventType::ACCEPT값 넣어줌
	AcceptEvent() : IocpEvent(EventType::ACCEPT) {}
};
