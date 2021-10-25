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

        //받는 버퍼 할당
        RecvBuffer recvBuffer = new RecvBuffer(1024);

        SocketAsyncEventArgs receiveArgs = new SocketAsyncEventArgs();
        SocketAsyncEventArgs sendArgs = new SocketAsyncEventArgs();

        Queue<ArraySegment<byte>> sendQueue = new Queue<ArraySegment<byte>>();

        bool isProgressing = false;
        object lockObj = new object();
        List<ArraySegment<byte>> sendList = new List<ArraySegment<byte>>();

        public abstract void OnConnected(EndPoint endPoint);

        //얼마만큼 데이터를 처리 했는지 크기값 반환
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
                //사용할 공간 확보
                recvBuffer.Clean();

                //쓸수있는 공간 받음
                ArraySegment<byte> segment = recvBuffer.FreeSegment;

                //쓸수 있는 공간 등록
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
                    //WritePos 이동
                    //args.BytesTransferred 받은 사이즈가 쓸수 있는 공간 보다 크다면
                    if (recvBuffer.OnWrite(args.BytesTransferred) == false)
                    {
                        //연결 끊기
                        Disconnect();
                        return;
                    }

                    //OnReceive에(처리해야할 데이터 뭉텅이를 보냄)
                    //거기서 계산해서 처리한 데이터의 크기값을 반환
                    int processLength = OnReceive(recvBuffer.DataSegment);
                    //처리한 데이터가 0보다 작으면??? 먼가 이상하니까
                    if (processLength < 0)
                    {
                        //연결 끊기
                        Disconnect();
                        return;
                    }


                    //ReadPos 이동
                    //처리해야할 데이터 보다 처리한 데이터가 크다면 먼가 이상하니까
                    if (recvBuffer.OnRead(processLength) == false)
                    {
                        //연결 끊기
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
