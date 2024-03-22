#pragma once
#include "Session.h"

//[2byte : size][2byte : id][���� ������...]
struct PacketHeader
{
	UINT16 size;	//ũ�� 2byte
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
	//sealed : ��� ���� �ڽ��� �� �Լ��� �������̵�(�����) �Ҽ� ����
	virtual int OnRecv(BYTE* buffer, int len) ;
	virtual int OnRecvPacket(BYTE* buffer, int len) abstract;
};

