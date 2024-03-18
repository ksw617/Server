#pragma once
#include "IocpEvent.h"
//IocpObj�� ����Ʈ�����ͷ� ���� ����
class IocpObj : public enable_shared_from_this<IocpObj>
{
public:
	virtual HANDLE GetHandle() abstract;
	virtual void ObserveIO(IocpEvent* iocpEvent, int numOfBytes = 0) abstract;
};

