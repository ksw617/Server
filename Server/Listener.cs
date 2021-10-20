using System;
using System.Net;
using System.Net.Sockets;
using ServerCore;

namespace Server
{
    class Listener
    {
        Socket listenSocket;
        Func<Session> sessionFactory;

        public void Initialize(IPEndPoint iPEndPoint, Func<Session> session, int backlog = 100, int acceptCount = 10)
        {
            sessionFactory += session;

            listenSocket = new Socket(iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.Bind(iPEndPoint);
            //대기자
            listenSocket.Listen(backlog);

            //처리자
            for (int i = 0; i < acceptCount; i++)
            {
                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.Completed += AcceptCompleted;
                RegistAccept(args);
            }
        }

        void RegistAccept(SocketAsyncEventArgs args)
        {
            args.AcceptSocket = null;      
            bool pending = listenSocket.AcceptAsync(args);

            if (pending == false)
            {
                AcceptCompleted(null, args);
            }

        }

        void AcceptCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                Session session = sessionFactory.Invoke();
                session.Initialize(args.AcceptSocket);
                session.OnConnected(args.AcceptSocket.RemoteEndPoint);
               
            }
            else 
            {
                Console.WriteLine(args.SocketError.ToString());
            }

            RegistAccept(args);
        }
    }
}
