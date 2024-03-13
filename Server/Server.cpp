#include "pch.h"
#include <IocpCore.h>
#include <ServerService.h>
#include <Session.h>

class ServerSession : public Session
{
	virtual void OnConnected() override 
	{
		cout << "Server Session" << endl;
	}

};



int main()
{
	printf("============= SERVER =============\n");

	//ServerService(ip주소, port번호, (함수))
	Service* service = new ServerService(L"127.0.0.1", 27015, []() {return new ServerSession; });
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
