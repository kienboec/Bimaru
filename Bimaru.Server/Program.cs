using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Bimaru.Logic;
using Bimaru.Logic.Local;
using Bimaru.Logic.RemoteObjects;

namespace Bimaru.Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            List<Task> tasks = new List<Task>();
            TcpListener listener = new TcpListener(IPAddress.Loopback, 8000);
            listener.Start(5);

            Console.CancelKeyPress += (sender, e) => Environment.Exit(0);

            try
            {
                while (true)
                {
                    var socket = await listener.AcceptTcpClientAsync();
                    tasks.Add(Task.Run(() => SingleConnection(socket)));
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("error occurred: " + exc.Message);
            }

            Task.WaitAll(tasks.ToArray());
        }

        public static void SingleConnection(TcpClient client)
        {
            Action<string> debug = message => Console.WriteLine(message);
            Action<Pitch> pitchDebug = pitch => Console.WriteLine(pitch.ToString());

            debug("starting connection...");

            RawPitchProvider provider = new RawPitchProvider();
            Pitch remoteObject = provider.GetNextPitch() as Pitch;
            if (remoteObject == null)
            {
                return;
            }

            using var writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
            using var reader = new StreamReader(client.GetStream());

            string message = null;
            do
            {
                try
                {
                    debug("waiting for data to read...");
                    message = reader.ReadLine();
                    debug($"received: " + message);
                    switch (Enum.Parse<ServerCommands>(message))
                    {
                        case ServerCommands.GET:
                            debug("   send pitch");
                            pitchDebug(remoteObject);
                            writer.WriteLine(remoteObject.ToString());
                            break;
                        case ServerCommands.TOGGLE:
                            debug("   read index");
                            message = reader.ReadLine();
                            debug($"   received: " + message);
                            remoteObject.Toggle(int.Parse(message));
                            pitchDebug(remoteObject);
                            writer.WriteLine(remoteObject.ToString());
                            debug("   send pitch");
                            break;
                        case ServerCommands.SOLVED:
                            var solvedAnswer = remoteObject.IsSolved().ToString();
                            debug($"   send {solvedAnswer}");
                            writer.WriteLine(solvedAnswer);
                            break;
                        case ServerCommands.QUIT:
                        default:
                            break;
                    }
                }
                catch (Exception exc)
                {
                    debug("exception: " + exc.Message);
                    if (!client.Connected)
                    {
                        break;
                    }
                }
            } while (message != ServerCommands.QUIT.ToString());

            client.Close();
        }
    }
}
