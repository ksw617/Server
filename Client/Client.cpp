#pragma once
#include "pch.h"
#include <CorePch.h>
#include <ClientService.h>
#include <Session.h>
#include <IocpCore.h>

char sendBuffer[] = "Hello This is Client";

class ClientSession : public Session
{
public:
	//소멸자 호출
	~ClientSession()
	{
		printf("ClientSession Destroy\n");
	}
	virtual void OnConnected() override
	{
		printf("Connected to Server\n");
		Send((BYTE*)sendBuffer, sizeof(sendBuffer));
	}

	virtual int OnRecv(BYTE* buffer, int len) override
	{
		printf("OnRecv : %s, OnRecv Len : %d\n", (char*)buffer, len);

		this_thread::sleep_for(1s);

		Send(buffer, len);

		return len;
	}

	virtual void OnSend(int len) override
	{
		printf("On Send Len : %d\n", len);
	}


	virtual void OnDisconnected()
	{
		printf("OnDisconnected\n");
	}

};

int main()
{
	this_thread::sleep_for(1s);

	shared_ptr<Service> service = make_shared<ClientService>(L"127.0.0.1", 27015, []() {return make_shared<ClientSession>(); });

		if (!service->Start())
		{
			printf("Server Start Error\n");
			return 1;
		}

		thread t([=]()
			{
				while (true)
				{
					service->GetIocpCore()->ObserveIO();
				}
			}
		);

		t.join();

		//필요 없음
		//delete service;

		return 0;
}

