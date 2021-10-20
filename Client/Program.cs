using System.Net;
using System.Threading;

namespace Client
{
    class Program
    {
        static Connector connector = new Connector();
        static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            IPHostEntry iPHostEntry = Dns.GetHostEntry(host);
            IPAddress iPAddress = iPHostEntry.AddressList[0];
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 777);

            connector.Initialize(iPEndPoint, () => { return new ServerSession(); });

            while (true)
            {
                
            }
        }
    }
}