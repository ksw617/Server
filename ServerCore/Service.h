#pragma once
#include <functional>

enum class ServiceType : u_char
{ 
	SERVER,
	CLIENT,
};


class IocpCore;
class Session;

//����Ʈ �����ͷ� ����
using SessionFactory = function<shared_ptr<Session>(void)>;

//Service�� ����Ʈ�����ͷ� ���� ����
class Service : public enable_shared_from_this<Service>
{
private:
	ServiceType serviceType;
	SOCKADDR_IN sockAddr = {};
	//iocpCore�� ���⼭ �Ҵ� ���ִϱ� ���� ���� ���ϰ� �ϱ� ���ؼ�
	shared_ptr<IocpCore> iocpCore = nullptr;
protected:
	shared_mutex rwLock;
	//����Ʈ �����ͷ� ����
	set<shared_ptr<Session>> sessions;
	int sessionCount = 0;
	SessionFactory sessionFactory;
public:
	Service(ServiceType type, wstring ip, u_short port, SessionFactory factory);
	virtual ~Service();
public:
	ServiceType GetServiceType() const { return serviceType; }
	SOCKADDR_IN& GetSockAddr() { return sockAddr; }
	//����Ʈ �����ͷ� ����
	shared_ptr<IocpCore> GetIocpCore() { return iocpCore; }
public:
	void SetSessionFactory(SessionFactory func) { sessionFactory = func; }
	//����Ʈ �����ͷ� ����
	shared_ptr<Session> CreateSession();
	//����Ʈ �����ͷ� ����
	void AddSession(shared_ptr<Session> session);
	//����Ʈ �����ͷ� ����
	void RemoveSession(shared_ptr<Session> session);
	int GetSessionCount() const { return sessionCount; }
public:
	virtual bool Start() abstract;

};
