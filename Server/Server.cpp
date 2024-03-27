#include "pch.h"
#include <IocpCore.h>
#include <ServerService.h>
#include <SendBufferManager.h>

#include "ClientSession.h"
#include "SessionManager.h"

#include "Protocol.pb.h"

#include "ClientPacketHandler.h"


#define THREAD_COUNT 5

int main()
{
	printf("============= SERVER =============\n");

	shared_ptr<Service> service = make_shared<ServerService>(L"127.0.0.1", 27015, []() {return make_shared<ClientSession>(); });
	if (!service->Start())
	{
		printf("Server Start Error\n");
		return 1;

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


	while (true)
	{
		Protocol::S_LOGIN	packet;

		shared_ptr<SendBuffer> sendBuffer = ClientPacketHandler::MakeSendBuffer(packet);

		if (sendBuffer != nullptr)
		{
			SessionManager::Get().Broadcast(sendBuffer);
		}

		this_thread::sleep_for(250ms);
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
