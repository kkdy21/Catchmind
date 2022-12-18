using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace PaintServer
{
    class Program
    {
        static void Main(string[] args)
        {
            AysncEchoServer().Wait();
        }
        async static Task AysncEchoServer()
        {
            TcpListener servsock = new TcpListener(IPAddress.Any, 81);
            TcpListener subservsock = new TcpListener(IPAddress.Any, 82);

            Flagprocess FP = new Flagprocess(subservsock);

            servsock.Start();

            while (true)
            {
                                
                TcpClient client = await servsock.AcceptTcpClientAsync().ConfigureAwait(false);
                
                Task t1 = new Task(() => FP.FlagHandleProcess(client));
                t1.Start();

            }
        }
    }
}