using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

namespace ServerCore
{
    public abstract class Session
    {
        Socket socket;
        int disconnected = 0;

        RecvBuffer recvBuffer = new RecvBuffer(1024);

        SocketAsyncEventArgs receiveArgs = new SocketAsyncEventArgs();
        SocketAsyncEventArgs sendArgs = new SocketAsyncEventArgs();

        Queue<ArraySegment<byte>> sendQueue = new Queue<ArraySegment<byte>>();

        bool isProgressing = false;
        object lockObj = new object();
        List<ArraySegment<byte>> sendList = new List<ArraySegment<byte>>();

        public abstract void OnConnected(EndPoint endPoint);
        public abstract int OnReceive(ArraySegment<byte> buffer);
        public abstract void OnSend(int numberOfBytes);
        public abstract void OnDisconnected(EndPoint endPoint);

        public void Initialize(Socket _socket)
        {
            this.socket = _socket;

            receiveArgs.Completed += ReceiveCompleted;
            sendArgs.Completed += SendCompleted;

            RegistReceive(receiveArgs);


        }

        void RegistReceive(SocketAsyncEventArgs args)
        {
            try
            {
                recvBuffer.Clean();

                ArraySegment<byte> segment = recvBuffer.FreeSegment;
                args.SetBuffer(segment.Array, segment.Offset, segment.Count);


                bool pending = socket.ReceiveAsync(args);

                if (pending == false)
                {
                    ReceiveCompleted(null, args);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"RegistReceive : {e.Message}");
            }
        }

        void ReceiveCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success && args.BytesTransferred > 0)
            {
                try
                {

                    if (recvBuffer.OnWrite(args.BytesTransferred) == false)
                    {
                        Disconnect();
                        return;
                    }
                    int processLength = OnReceive(recvBuffer.DataSegment);
                    if (processLength < 0)
                    {
                        Disconnect();
                        return;
                    }

                    if (recvBuffer.OnRead(processLength) == false)
                    {
                        Disconnect();
                        return;
                    }
                 

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

        public void Send(List<ArraySegment<byte>> sendBuffers)
        {
            if (sendBuffers.Count == 0)
            {
                return;
            }

            lock (lockObj)
            {
                foreach (ArraySegment<byte> sendBuffer in sendBuffers)
                {
                    sendQueue.Enqueue(sendBuffer);
                }

                if (!isProgressing)
                {
                    isProgressing = true;
                    RegistSend(sendArgs);
                }
            }
        }

        public void Send(ArraySegment<byte> sendBuffer)
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
                sendList.Add(sendQueue.Dequeue());
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

                        OnSend(args.BytesTransferred);

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
                OnDisconnected(socket.RemoteEndPoint);

                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }

        }
    }
}
