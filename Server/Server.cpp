#include "pch.h"
#include <IocpCore.h>
#include <ServerService.h>
#include <PacketSession.h>
#include <SendBufferManager.h>	 

class ServerSession : public PacketSession
{
public:
	~ServerSession()
	{
		printf("ServerSession Destroy\n");
	}
	virtual void OnConnected() override 
	{
		printf("OnConnected\n");
	}

	virtual int OnRecvPacket(BYTE* buffer, int len) override
	{

		shared_ptr<SendBuffer> sendBuffer = SendBufferManager::Get().Open(4096);
		memcpy(sendBuffer->GetBuffer(), buffer, len);
		  
		if (sendBuffer->Close(len))
		{
			Send(sendBuffer);
		}

		printf("%s\n", &buffer[4]);
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
	printf("============= SERVER =============\n");

	shared_ptr<Service> service = make_shared<ServerService>(L"127.0.0.1", 27015, []() {return make_shared<ServerSession>(); });
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

	return 0;


}
