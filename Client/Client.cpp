#pragma once
#include "pch.h"
#include <CorePch.h>
#include <ClientService.h>
#include <PacketSession.h>
#include <IocpCore.h>
#include <SendBufferManager.h>

char sendData[] = "Hello world";

class ClientSession : public PacketSession
{
public:
	//소멸자 호출
	~ClientSession()
	{
		printf("ClientSession Destroy\n");
	}
	virtual void OnConnected() override
	{
		shared_ptr<SendBuffer> sendBuffer = SendBufferManager::Get().Open(4096);

		BYTE* data = sendBuffer->GetBuffer();

		int sendSize = sizeof(PacketHeader) + sizeof(sendData);
		((PacketHeader*)data)->size = sendSize;
		((PacketHeader*)data)->id = 0;

		memcpy(&data[4], sendData, sizeof(sendData));

		if (sendBuffer->Close(sendSize))
		{
			Send(sendBuffer);
		}
	}

	virtual int OnRecvPacket(BYTE* buffer, int len) override
	{

		this_thread::sleep_for(1s);

		shared_ptr<SendBuffer> sendBuffer = SendBufferManager::Get().Open(4096);

		BYTE* data = sendBuffer->GetBuffer();

		//14byte     =    4byte				+		sendData[12]
		int sendSize = sizeof(PacketHeader) + sizeof(sendData);
		//[size(2) = sendSize]										4096]
		((PacketHeader*)data)->size = sendSize;
		//[size(2) = sendSize][id(2) = 0]							4096]
		((PacketHeader*)data)->id = 0;
		//[size(2) = sendSize][id(2) = 0][sendData = "Hello world"NULL]4096]
		memcpy(&data[4], sendData, sizeof(sendData));
		//[[size(2)][id(2)][sendData(12)]]
		if (sendBuffer->Close(sendSize))
		{
			Send(sendBuffer);
		}


		return len;
	}

	virtual void OnSend(int len) override
	{
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
	for (int i = 0; i < 1; i++)
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

