#pragma once

class IocpCore;
class Listener;

class Service
{
private:
	SOCKADDR_IN sockAddr = {};
	Listener* listener = nullptr;
	IocpCore* iocpCore = nullptr;
public:
	Service(wstring ip, u_short port);
	~Service();
public:
	SOCKADDR_IN& GetSockAddr() { return sockAddr; }
	IocpCore* GetIocpCore() { return iocpCore; }
public:
	bool Start();
};
