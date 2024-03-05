#include "pch.h"
#include <Service.h>
#include <IocpCore.h>


int main()
{
	printf("============= SERVER =============\n");

	//���⼭ �����Ҷ����� ���
	Service* service = new Service(L"127.0.0.1", 27015);
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
