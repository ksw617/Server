#include "pch.h"
#include <IocpCore.h>
#include <ServerService.h>
#include <Session.h>

class ServerSession : public Session
{
	//Todo

};



int main()
{
	printf("============= SERVER =============\n");

	//저기서 접속할때까지 대기
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
