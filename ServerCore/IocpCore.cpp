#include "pch.h"
#include "IocpCore.h"
#include "IocpEvent.h"
#include "IocpObj.h"

IocpCore::IocpCore()
{
	//생성
	iocpHandle = CreateIoCompletionPort(INVALID_HANDLE_VALUE, NULL, NULL, NULL);
}

IocpCore::~IocpCore()
{
	CloseHandle(iocpHandle);
}

void IocpCore::Register(class IocpObj* iocpObj)
{
	CreateIoCompletionPort(iocpObj->GetHandle(), iocpHandle, 0, 0);
}

bool IocpCore::ObserveIO(DWORD time)
{
	DWORD bytesTransferred = 0;
	ULONG_PTR key = 0;
	IocpEvent* iocpEvent = nullptr;

	if (GetQueuedCompletionStatus(iocpHandle, &bytesTransferred, &key, (LPOVERLAPPED*)&iocpEvent, time))
	{
		IocpObj* iocpObj = iocpEvent->iocpObj;

		//까보니까 Listener네
		//Listener의 ObserveIO 함수 실행
		//AcceptEvent 넘겨주고 - 연결되어 있는 Session, Listener
		// bytesTransferred 얼마나 받았는지
		iocpObj->ObserveIO(iocpEvent, bytesTransferred);
	}
	else
	{
		switch (GetLastError())
		{
		case WAIT_TIMEOUT:
			return false;
		default:
			break;
		}

		return false;
	}


	//Todo

	return true;
}
