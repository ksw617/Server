using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace ServerCore
{
    public class Session
    {
        Socket socket;
        int disconnected = 0;

        SocketAsyncEventArgs receiveArgs = new SocketAsyncEventArgs();
        SocketAsyncEventArgs sendArgs = new SocketAsyncEventArgs();

        Queue<byte[]> sendQueue = new Queue<byte[]>();
        bool isProgressing = false;

        object lockObj = new object();
        List<ArraySegment<byte>> sendList = new List<ArraySegment<byte>>();

        public void Initialize(Socket _socket)
        {
            this.socket = _socket;

            receiveArgs.Completed += ReceiveCompleted;
            sendArgs.Completed += SendCompleted;

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
            lock (lockObj)
            {
                sendQueue.Enqueue(sendBuffer);

                if (!isProgressing)
                {
                    isProgressing = true;
                    RegistSend(sendArgs);
                }
            }


        }

        void RegistSend(SocketAsyncEventArgs args)
        {
            while (sendQueue.Count > 0)
            {
                byte[] sendBuffer = sendQueue.Dequeue();
                sendList.Add(new ArraySegment<byte>(sendBuffer, 0, sendBuffer.Length));
            }

            args.BufferList = sendList;

            bool pennding = socket.SendAsync(args);

            if (pennding == false)
            {
                SendCompleted(null, args);
            }
        }

        void SendCompleted(object sender, SocketAsyncEventArgs args)
        {
            lock (lockObj)
            {
                if (args.SocketError == SocketError.Success && args.BytesTransferred > 0)
                {
                    try
                    {
                        args.BufferList = null;
                        sendList.Clear();

                        if (sendQueue.Count > 0)
                        {
                            RegistSend(args);
                        }
                        else
                        {
                            isProgressing = false;
                        }
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
        }

        public void Disconnect()
        {
            if (Interlocked.Exchange(ref disconnected, 1) == 0)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }

        }
    }
}
