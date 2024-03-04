#include "pch.h"
#include "IocpEvent.h"

IocpEvent::IocpEvent(EventType type) : eventType(type)
{
	//eventType = type;
	Init();
}

void IocpEvent::Init()
{
	//부모에 있는::
	OVERLAPPED::hEvent = NULL;	//0
	OVERLAPPED::Internal = NULL;
	OVERLAPPED::InternalHigh = NULL;
	OVERLAPPED::Offset = NULL;
	OVERLAPPED::OffsetHigh = NULL;
	OVERLAPPED::Pointer = NULL;
}
