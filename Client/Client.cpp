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
		//서버 연결을 시도
		if (connect(connectSocket, (SOCKADDR*)&service, sizeof(service)) == SOCKET_ERROR)
		{
			//비동기 연결 시도 중 발생하는 일반적인 오류들 처리
			if (WSAGetLastError() == WSAEWOULDBLOCK || WSAGetLastError() == WSAEALREADY)
			{
				continue;
			}
					  
			//연결이 완료되었는지 확인
			if (WSAGetLastError() == WSAEISCONN)
			{
				break;
			}

			//연결 실패 시 오류 메세지 출력
			printf("connect failed with error %d\n", WSAGetLastError());
			closesocket(connectSocket);
			WSACleanup();
			return 1;
		}

	}

	//서버 연결 성공시 메세지 출력
	printf("Connected to Server!\n");

	char sendBuffer[100] = "Hello this is client!"; //전송할 메세지

	//데이터 전송을 위한 준비
	WSAEVENT wsaEvent = WSACreateEvent();		//win sock 이벤트 객체를 생성
	WSAOVERLAPPED overlapped = {};				//비동기 I/O 작업을 위한 구조체 초기화
	overlapped.hEvent = wsaEvent;				//overlapped 구조체에 이벤트 객체를 할당
			  
	//데이터 전송 루프
	while (true)
	{

		WSABUF wsaBuf;					//WSABUF 구조체를 선언, winsock 함수에 사용할 버퍼를 관리
		wsaBuf.buf = sendBuffer;		//버퍼 포인터를 sendBuffer로 설정
		wsaBuf.len = sizeof(sendBuffer);//버퍼의 길이 설정

		DWORD sendLen = 0;				//전송된 데이터의 길이를 저장할 변수
		DWORD flags = 0;				//전송 시 사용할 추가 옵션 플래그. 현재는 사용 안함

		//WSASend 함수를 사용하여 데이터를 비동기적으로 전송
		if (WSASend(connectSocket, &wsaBuf, 1, &sendLen, flags, &overlapped, nullptr) == SOCKET_ERROR)
		{
			//WAS_IO_PENDING 에러는 비동기 작업이 대기 중임을 의미
			if (WSAGetLastError() == WSA_IO_PENDING)
			{
				//비동기 작업이 완료될 때까지 대기
				WSAWaitForMultipleEvents(1, &wsaEvent, TRUE, WSA_INFINITE, FALSE);

				//비동기 작업의 결과를 가져옴. 여기서 실제로 데이터가 전송됨
				WSAGetOverlappedResult(connectSocket, &overlapped, &sendLen, FALSE, &flags);
			}
			else // 진짜 오류
			{
				//다른 오류가 발생한 경우 루프 중단
				break;
			}

		}

		 //전송한 데이터의 크기를 출력
		printf("Send Buffer Length : %d bytes\n", sizeof(sendBuffer));
		Sleep(1000);
	}


	WSACloseEvent(wsaEvent); // 사용한 이벤트 객체를 닫음
	closesocket(connectSocket);
	WSACleanup();
}

