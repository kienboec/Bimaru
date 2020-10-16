using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Bimaru.Logic
{
    public class RemotePitchProvider : IPitchProvider
    {
        public string GetNextPitchRaw()
        {
            using TcpClient client = new TcpClient("localhost", 8000);
            using StreamReader reader = new StreamReader(client.GetStream());
            return reader.ReadToEnd();
        }
    }
}
