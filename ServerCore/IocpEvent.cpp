#include "pch.h"
#include "IocpEvent.h"

IocpEvent::IocpEvent(EventType type) : eventType(type)
{
	//eventType = type;
	Init();
}

void IocpEvent::Init()
{
	//�θ� �ִ�::
	OVERLAPPED::hEvent = NULL;	//0
	OVERLAPPED::Internal = NULL;
	OVERLAPPED::InternalHigh = NULL;
	OVERLAPPED::Offset = NULL;
	OVERLAPPED::OffsetHigh = NULL;
	OVERLAPPED::Pointer = NULL;
}
