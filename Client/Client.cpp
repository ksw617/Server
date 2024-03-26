#pragma once
#include "pch.h"
#include <CorePch.h>
#include <ClientService.h>
#include <PacketSession.h>
#include <IocpCore.h>

#include "Protocol.pb.h"

class ServerSession : public PacketSession
{
public:
	//º“∏Í¿⁄ »£√‚
	~ServerSession()
	{
		printf("ClientSession Destroy\n");
	}
	virtual void OnConnected() override
	{
	}

	virtual int OnRecvPacket(BYTE* buffer, int len) override
	{
		Protocol::TEST packet;
		packet.ParseFromArray(buffer + sizeof(PacketHeader), len - sizeof(PacketHeader));
		
		printf("ID : %d, HP : %d\n", packet.id(), packet.hp());

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

#define THREAD_COUNT 2

int main()
{
	this_thread::sleep_for(1s);
	printf("============= CLIENT =============\n");

	shared_ptr<Service> service = make_shared<ClientService>(L"127.0.0.1", 27015, []() {return make_shared<ServerSession>(); });


	for (int i = 0; i < 1; i++)
	{
		if (!service->Start())
		{
			printf("Server Start Error\n");
			return 1;
		}
	}

	vector<thread> threads;

	for (int i = 0; i < THREAD_COUNT; i++)
	{
		threads.push_back(thread([=]()
			{
				while (true)
				{
					service->GetIocpCore()->ObserveIO();
				}
			}
		));
	}


	for (int i = 0; i < THREAD_COUNT; i++)
	{
		if (threads[i].joinable())
		{
			threads[i].join();
		}

	}
	

	return 0;
}

