using System;
using System.Net;
using ServerCore;
using System.Collections.Generic;

namespace Server
{
    class Program
    {
        static Listener listener = new Listener();
        static List<System.Timers.Timer> timers = new List<System.Timers.Timer>();

        static void SetTickTime(Action action, int tick = 100)
        {
            var timer = new System.Timers.Timer();
            timer.Interval = tick;
            timer.Elapsed += (s, e) => { action.Invoke(); };
            timer.AutoReset = true;
            timer.Enabled = true;

            timers.Add(timer);
        }

        static void Main(string[] args)
        {

            //?로비를 어떻게 할지 고민
            Lobby lobby = Lobby.Instance;
            SetTickTime(lobby.Update, 50);

            string host = Dns.GetHostName();
            IPHostEntry iPHostEntry = Dns.GetHostEntry(host);
            IPAddress iPAddress = iPHostEntry.AddressList[0];

            Console.WriteLine(iPHostEntry.AddressList[0]);
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 777);
            Console.WriteLine(iPEndPoint);

            listener.Initialize(iPEndPoint, () => { return SessionManager.Instance.Create(); });

            while (true)
            {

            }

        }
    }
}
