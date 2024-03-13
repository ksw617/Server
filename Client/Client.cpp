#include <iostream>			   
using namespace std;

#pragma comment(lib, "Ws2_32.lib")
#include <WinSock2.h>
#include <WS2tcpip.h>


int main()
{
	Sleep(1000);

	printf("============= CLIENT =============\n");

	WORD wVersionRequested;
	WSAData wsaData;

	wVersionRequested = MAKEWORD(2, 2);
	if (WSAStartup(wVersionRequested, &wsaData) != 0)
	{
		printf("WSAStartup function failed with error\n");
		return 1;
	}


	SOCKET connectSocket = WSASocket(AF_INET, SOCK_STREAM, IPPROTO_TCP, NULL, 0, WSA_FLAG_OVERLAPPED);
	if (connectSocket == INVALID_SOCKET)
	{
		printf("socket function failed with error : %d\n", WSAGetLastError());
		WSACleanup();
		return 1;

	}

	u_long iMode = 1;
	if (ioctlsocket(connectSocket, FIONBIO, &iMode) == INVALID_SOCKET)
	{
		printf("ioctlsocket failed with error : %d\n", WSAGetLastError());
		closesocket(connectSocket);
		WSACleanup();
		return 1;
	}

	SOCKADDR_IN service;
	memset(&service, 0, sizeof(service));
	service.sin_family = AF_INET;
	inet_pton(AF_INET, "127.0.0.1", &service.sin_addr);
	service.sin_port = htons(27015);


	while (true)
	{
		//���� ������ �õ�
		if (connect(connectSocket, (SOCKADDR*)&service, sizeof(service)) == SOCKET_ERROR)
		{
			//�񵿱� ���� �õ� �� �߻��ϴ� �Ϲ����� ������ ó��
			if (WSAGetLastError() == WSAEWOULDBLOCK || WSAGetLastError() == WSAEALREADY)
			{
				continue;
			}
					  
			//������ �Ϸ�Ǿ����� Ȯ��
			if (WSAGetLastError() == WSAEISCONN)
			{
				break;
			}

			//���� ���� �� ���� �޼��� ���
			printf("connect failed with error %d\n", WSAGetLastError());
			closesocket(connectSocket);
			WSACleanup();
			return 1;
		}

	}

	//���� ���� ������ �޼��� ���
	printf("Connected to Server!\n");

	char sendBuffer[100] = "Hello this is client!"; //������ �޼���

	//������ ������ ���� �غ�
	WSAEVENT wsaEvent = WSACreateEvent();		//win sock �̺�Ʈ ��ü�� ����
	WSAOVERLAPPED overlapped = {};				//�񵿱� I/O �۾��� ���� ����ü �ʱ�ȭ
	overlapped.hEvent = wsaEvent;				//overlapped ����ü�� �̺�Ʈ ��ü�� �Ҵ�
			  
	//������ ���� ����
	while (true)
	{

		WSABUF wsaBuf;					//WSABUF ����ü�� ����, winsock �Լ��� ����� ���۸� ����
		wsaBuf.buf = sendBuffer;		//���� �����͸� sendBuffer�� ����
		wsaBuf.len = sizeof(sendBuffer);//������ ���� ����

		DWORD sendLen = 0;				//���۵� �������� ���̸� ������ ����
		DWORD flags = 0;				//���� �� ����� �߰� �ɼ� �÷���. ����� ��� ����

		//WSASend �Լ��� ����Ͽ� �����͸� �񵿱������� ����
		if (WSASend(connectSocket, &wsaBuf, 1, &sendLen, flags, &overlapped, nullptr) == SOCKET_ERROR)
		{
			//WAS_IO_PENDING ������ �񵿱� �۾��� ��� ������ �ǹ�
			if (WSAGetLastError() == WSA_IO_PENDING)
			{
				//�񵿱� �۾��� �Ϸ�� ������ ���
				WSAWaitForMultipleEvents(1, &wsaEvent, TRUE, WSA_INFINITE, FALSE);

				//�񵿱� �۾��� ����� ������. ���⼭ ������ �����Ͱ� ���۵�
				WSAGetOverlappedResult(connectSocket, &overlapped, &sendLen, FALSE, &flags);
			}
			else // ��¥ ����
			{
				//�ٸ� ������ �߻��� ��� ���� �ߴ�
				break;
			}

		}

		 //������ �������� ũ�⸦ ���
		printf("Send Buffer Length : %d bytes\n", sizeof(sendBuffer));
		Sleep(1000);
	}


	WSACloseEvent(wsaEvent); // ����� �̺�Ʈ ��ü�� ����
	closesocket(connectSocket);
	WSACleanup();
}

