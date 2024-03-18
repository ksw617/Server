#pragma once
#include "Service.h"
class ServerService	: public Service
{
private:
	//스마트 포인터로 변환
	shared_ptr<class Listener> listener = nullptr;
public:
	ServerService(wstring ip, u_short port, SessionFactory factory);
	virtual ~ServerService() {}
public:
	virtual bool Start() override;

};

