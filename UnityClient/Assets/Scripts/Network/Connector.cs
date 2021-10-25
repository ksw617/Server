using System;
using System.Net;
using System.Net.Sockets;
using ServerCore;

class Connector
{
    Func<Session> sessionFactory;
    public void Initialize(IPEndPoint iPEndPoint, Func<Session> session)
    {

        sessionFactory += session;

        Socket clientSocket = new Socket(iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        SocketAsyncEventArgs args = new SocketAsyncEventArgs();
        args.Completed += ConnectCompleted;
        args.RemoteEndPoint = iPEndPoint;

        args.UserToken = clientSocket;

        RegistConnect(args);

    }

    void RegistConnect(SocketAsyncEventArgs args)
    {
        Socket clientSocket = (Socket)args.UserToken;

        if (clientSocket == null)
        {
            return;
        }

        bool pending = clientSocket.ConnectAsync(args);

        if (pending == false)
        {
            ConnectCompleted(null, args);
        }
    }

    void ConnectCompleted(object sender, SocketAsyncEventArgs args)
    {
        if (args.SocketError == SocketError.Success)
        {
            //sessionFactory에서 반환되는 Session을 상속받은 커스터마이징된 Session을 참조
            Session session = sessionFactory.Invoke();
            session.Initialize(args.ConnectSocket);
            session.OnConnected(args.RemoteEndPoint);
        }
        else
        {
            Console.WriteLine(args.SocketError.ToString());
        }
    }

}
