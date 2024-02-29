#pragma once
class Listener
{
private:
	SOCKET socket = INVALID_SOCKET;
public:
	Listener() = default;
	~Listener();
public:
public:
	bool StartAccept(class Service* service);
	void CloseSocket();
};

