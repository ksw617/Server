using System;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class Listener
    {
        Socket listenSocket;

        Action<Socket> AcceptHandler;

        public void Initialize(IPEndPoint iPEndPoint, Action<Socket> action)
        {
            listenSocket = new Socket(iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            listenSocket.Bind(iPEndPoint);
            listenSocket.Listen(10);

            AcceptHandler = action;

            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.Completed += AcceptCompleted;
            RegistAccept(args);

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

                AcceptHandler.Invoke(args.AcceptSocket);
            }
            else 
            {
                Console.WriteLine(args.SocketError.ToString());
            }

            RegistAccept(args);
        }
    }
}
