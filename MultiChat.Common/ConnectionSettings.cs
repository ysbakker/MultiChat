using System.Net;

namespace MultiChat.Common.Models
{
    public class ConnectionSettingsModel
    {
        public string Name { get; }
        public IPAddress IPAddress { get; }
        public int Port { get; }
        public int BufferSize { get; }

        public ConnectionSettingsModel(string name, IPAddress ipAddress, int port, int bufferSize)
        {
            Name = name;
            IPAddress = ipAddress;
            Port = port;
            BufferSize = bufferSize;
        }
        
        public ConnectionSettingsModel(string name, int port, int bufferSize)
        {
            Name = name;
            IPAddress = IPAddress.Any;
            Port = port;
            BufferSize = bufferSize;
        }
    }
}