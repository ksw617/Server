using System;
using System.Net.Sockets;
using System.Text;
using System.Threading; // 사용

namespace ServerCore
{
    public class Session
    {
        Socket socket;

        //종료 flag
        int disconnected = 0;

        public void Initialize(Socket _socket)
        {
            this.socket = _socket;

            SocketAsyncEventArgs receiveArgs = new SocketAsyncEventArgs();
            receiveArgs.Completed += ReceiveCompleted;

            receiveArgs.SetBuffer(new byte[1024], 0, 1024);
            RegistReceive(receiveArgs);
        }

        void RegistReceive(SocketAsyncEventArgs args)
        {
            bool pending = socket.ReceiveAsync(args);

            if (pending == false)
            {
                ReceiveCompleted(null, args);
            }
        }

        void ReceiveCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success && args.BytesTransferred > 0)
            {
                try
                {
                    string recvData = Encoding.UTF8.GetString(args.Buffer, args.Offset, args.BytesTransferred);
                    Console.WriteLine(recvData);

                    RegistReceive(args);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else 
            {
                Disconnect();
            }
            
        }

        public void Send(byte[] sendBuffer)
        {
            socket.Send(sendBuffer);
        }

        public void Disconnect()
        {
            //Interlocked.Exchange 반환되는데 ref 변수가 원래 가지고 있는값
            //Interlocked.Exchange(ref 변수, 바꾸고 싶은값)
            if (Interlocked.Exchange(ref disconnected, 1) == 0)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }

        }

    }
}
