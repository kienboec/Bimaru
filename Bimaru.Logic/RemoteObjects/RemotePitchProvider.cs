using System.IO;
using System.Net.Sockets;
using System.Text;
using Bimaru.Logic.Local;

namespace Bimaru.Logic.RemoteObjects
{
    public class RemotePitchProvider : IPitchProvider
    {
        private TcpClient _client;
        private StreamReader _reader;
        private StreamWriter _writer;

        public RemotePitchProvider()
        {
            _client = new TcpClient("localhost", 8000);
            _writer = new StreamWriter(_client.GetStream()){AutoFlush = true};
            _reader = new StreamReader(_client.GetStream());
        }

        public IPitch GetNextPitch()
        {
            _writer.WriteLine(ServerCommands.GET.ToString());
            return new RemotePitch(this, ReadPitchRawPitchFromSocket());
        }

        private string ReadPitchRawPitchFromSocket()
        {
            StringBuilder builder = new StringBuilder(100);
            string line;
            while ((line = _reader.ReadLine()) != string.Empty)
            {
                builder.AppendLine(line);
            }

            return builder.ToString();
        }

        public void Close()
        {
            this?._writer?.WriteLine(ServerCommands.QUIT.ToString());
            _client?.Close();
            _client = null;
        }


        public void Dispose()
        {
            Close();
        }

        public Pitch SendToggle(in int index)
        {
            this._writer.WriteLine(ServerCommands.TOGGLE.ToString());
            this._writer.WriteLine($"{index}");
            return new Pitch(ReadPitchRawPitchFromSocket());
        }

        public bool SendSolvedCheck()
        {
            this._writer.WriteLine(ServerCommands.SOLVED.ToString());
            var answer = _reader.ReadLine();
            bool returnValue = bool.Parse(answer);
            return returnValue;
        }
    }
}
