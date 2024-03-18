#include "pch.h"
#include "ServerService.h"
#include "Listener.h"

ServerService::ServerService(wstring ip, u_short port, SessionFactory factory) : Service(ServiceType::SERVER, ip, port, factory)
{
}

bool ServerService::Start()
{
    listener = make_shared<Listener>();
    //shared_from_this()-> Service�� �ּ� -> static_pointer_cast<ServerService>(Service�� �ּ�) -> ServerService�� �ּ� 
    return (listener->StartAccept(static_pointer_cast<ServerService>(shared_from_this())));
}
