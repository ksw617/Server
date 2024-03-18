#pragma once
class IocpCore
{
private:
	HANDLE iocpHandle = NULL;
public:
	IocpCore();
	~IocpCore();
public:
	HANDLE GetHandle() const { return iocpHandle; }
public:
	//스마트 포인터로 변환
	bool Register(shared_ptr<class IocpObj> iocpObj);  
	bool ObserveIO(DWORD time = INFINITE);
};

