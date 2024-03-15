#include "pch.h"
#include "Service.h"
#include "SocketHelper.h"
#include "IocpCore.h"
#include "Session.h"
																 //Session* Factor() { return new ClientSession; }
																//factor() -> new ClientSession
Service::Service(ServiceType type, wstring ip, u_short port, SessionFactory factory) : serviceType(type), sessionFactory(factory)
{
	if (!SocketHelper::StartUp())
		return;

	memset(&sockAddr, 0, sizeof(sockAddr));
	sockAddr.sin_family = AF_INET;	//Ipv4

	IN_ADDR address;
	InetPton(AF_INET, ip.c_str(), &address); //127.0.0.1
	sockAddr.sin_addr = address;
	sockAddr.sin_port = htons(port); //27015

	iocpCore = new IocpCore;  //
	sessionCount = 0;
}

Service::~Service()
{
	if (iocpCore != nullptr)
	{
		SocketHelper::CleanUp();
		delete iocpCore;
		iocpCore = nullptr;
	}

}

Session* Service::CreateSession()
{				
	Session* session = sessionFactory();
	session->SetService(this);   // 여기로 변경

	if (!iocpCore->Register(session))
	{
		return nullptr;
	}

	return session;
}

void Service::AddSession(Session* session)
{
	unique_lock<shared_mutex> lock(rwLock);
	sessionCount++;
	sessions.insert(session);
}

void Service::RemoveSession(Session* session)
{
	unique_lock<shared_mutex> lock(rwLock);
	sessions.erase(session);
	sessionCount--;
}
