#pragma once
#include "pch.h"
#include <CorePch.h>
#include <ClientService.h>
#include <Session.h>
#include <IocpCore.h>

char sendBuffer[] = "Hello This is Client";

class ClinetSession : public Session
{
public:
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
	Service* service = new ClientService(L"127.0.0.1", 27015, []() {return new ClinetSession; });

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

		delete service;

		return 0;
}

