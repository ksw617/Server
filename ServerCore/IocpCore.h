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
	//����Ʈ �����ͷ� ��ȯ
	bool Register(shared_ptr<class IocpObj> iocpObj);  
	bool ObserveIO(DWORD time = INFINITE);
};

