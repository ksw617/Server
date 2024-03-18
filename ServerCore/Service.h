#pragma once
#include <functional>

enum class ServiceType : u_char
{ 
	SERVER,
	CLIENT,
};


class IocpCore;
class Session;

//스마트 포인터로 관리
using SessionFactory = function<shared_ptr<Session>(void)>;

//Service를 스마트포인터로 레퍼 관리
class Service : public enable_shared_from_this<Service>
{
private:
	ServiceType serviceType;
	SOCKADDR_IN sockAddr = {};
	//iocpCore도 여기서 할당 해주니까 레퍼 관리 편하게 하기 위해서
	shared_ptr<IocpCore> iocpCore = nullptr;
protected:
	shared_mutex rwLock;
	//스마트 포인터로 관리
	set<shared_ptr<Session>> sessions;
	int sessionCount = 0;
	SessionFactory sessionFactory;
public:
	Service(ServiceType type, wstring ip, u_short port, SessionFactory factory);
	virtual ~Service();
public:
	ServiceType GetServiceType() const { return serviceType; }
	SOCKADDR_IN& GetSockAddr() { return sockAddr; }
	//스마트 포인터로 관리
	shared_ptr<IocpCore> GetIocpCore() { return iocpCore; }
public:
	void SetSessionFactory(SessionFactory func) { sessionFactory = func; }
	//스마트 포인터로 관리
	shared_ptr<Session> CreateSession();
	//스마트 포인터로 관리
	void AddSession(shared_ptr<Session> session);
	//스마트 포인터로 관리
	void RemoveSession(shared_ptr<Session> session);
	int GetSessionCount() const { return sessionCount; }
public:
	virtual bool Start() abstract;

};
