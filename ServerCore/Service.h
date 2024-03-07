#pragma once

enum class ServiceType : u_char
{ 
	SERVER,
	CLIENT,
};

class IocpCore;

class Service
{
private:
	ServiceType serviceType;
	SOCKADDR_IN sockAddr = {};
	IocpCore* iocpCore = nullptr;
protected:
	shared_mutex rwLock;
public:
	Service(ServiceType type, wstring ip, u_short port);
	virtual ~Service();
public:
	SOCKADDR_IN& GetSockAddr() { return sockAddr; }
	IocpCore* GetIocpCore() { return iocpCore; }
public:
	virtual bool Start() abstract;
};
