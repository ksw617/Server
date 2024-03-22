#pragma once
#include "Session.h"

//[2byte : size][2byte : id][보낼 데이터...]
struct PacketHeader
{
	UINT16 size;	//크기 2byte
	UINT16 id;		//id   2byte
};

class PacketSession	: public Session
{
public:
	PacketSession() {}
	virtual ~PacketSession() {}
public:
	shared_ptr<PacketSession> GetPacketSession() { return static_pointer_cast<PacketSession>(shared_from_this()); }
public:
	//sealed : 상속 받은 자식이 이 함수를 오버라이드(덮어쓰기) 할수 없음
	virtual int OnRecv(BYTE* buffer, int len) ;
	virtual int OnRecvPacket(BYTE* buffer, int len) abstract;
};

