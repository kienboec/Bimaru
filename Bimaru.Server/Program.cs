using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Bimaru.Logic;

namespace Bimaru.Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IPitchProvider provider = new RawPitchProvider();

            TcpListener listener = new TcpListener(IPAddress.Loopback, 8000);
            listener.Start(5);

            Console.CancelKeyPress += (sender, e) => Environment.Exit(0);

            while (true)
            {
                try
                {
                    var socket = await listener.AcceptTcpClientAsync();
                    using var writer = new StreamWriter(socket.GetStream()) { AutoFlush = true };
                    writer.WriteLine(provider.GetNextPitchRaw());
                    socket.Close();
                }
                catch (Exception exc)
                {
                    Console.WriteLine("error occurred: " + exc.Message);
                }
            }
        }
    }
}
