#pragma once
#include "pch.h"
#include <CorePch.h>
#include <ClientService.h>
#include <Session.h>
#include <IocpCore.h>
#include <SendBufferManager.h>

char sendData[] = "Hello This is Client";

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
		//printf("Connected to Server\n");
		shared_ptr<SendBuffer> sendBuffer = SendBufferManager::Get().Open(4096);
		memcpy(sendBuffer->GetBuffer(), sendData, sizeof(sendData));
		if (sendBuffer->Close(sizeof(sendData)))
		{
			Send(sendBuffer);
		}
	}

	virtual int OnRecv(BYTE* buffer, int len) override
	{
		//printf("OnRecv : %s, OnRecv Len : %d\n", (char*)buffer, len);

		this_thread::sleep_for(0.1s);


		shared_ptr<SendBuffer> sendBuffer = SendBufferManager::Get().Open(4096);
		memcpy(sendBuffer->GetBuffer(), buffer, len);
		if (sendBuffer->Close(len))
		{
			Send(sendBuffer);
		}


		return len;
	}

	virtual void OnSend(int len) override
	{
		//printf("On Send Len : %d\n", len);
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

	//1000명정도 접속 시작
	for (int i = 0; i < 1000; i++)
	{
		if (!service->Start())
		{
			printf("Server Start Error\n");
			return 1;
		}
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

	return 0;
}

