#include "pch.h"
#include <IocpCore.h>
#include <ServerService.h>
#include <Session.h>
#include <SendBufferManager.h>	 

class ServerSession : public Session
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

	virtual int OnRecv(BYTE* buffer, int len) override
	{
		//printf("OnRecv : %s, On Recv Len : %d\n", buffer, len);

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
		//printf("OnSend Len : %d\n", len);
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
