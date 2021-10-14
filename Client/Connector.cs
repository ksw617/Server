using System;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    class Connector
    {
        Action<Socket> ConnectHandler;
        public void Initialize(IPEndPoint iPEndPoint, Action<Socket> action)
        {

            ConnectHandler = action;
            //내부로 집어넣음
            Socket clientSocket = new Socket(iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.Completed += ConnectCompleted;
            args.RemoteEndPoint = iPEndPoint;

            //UserToken에 clientSocket을 연결
            args.UserToken = clientSocket;

            RegistConnect(args);

        }

        void RegistConnect(SocketAsyncEventArgs args)
        {
            //Socket clientSocket = args.UserToken as Socket;
            Socket clientSocket = (Socket)args.UserToken;

            //혹시나 clientSocket이 null이라면 return
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
                //args에 있는 ConnectSocket을 넘겨줌
                ConnectHandler.Invoke(args.ConnectSocket);
            }
            else 
            {
                Console.WriteLine(args.SocketError.ToString());
            }
        }

    }
}
