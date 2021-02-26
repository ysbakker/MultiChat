using System.Net;

namespace MultiChat.Common
{
    public class ConnectionSettings
    {
        public string Name { get; }
        public IPAddress IPAddress { get; }
        public int Port { get; }
        public int BufferSize { get; }

        public ConnectionSettings(string name, IPAddress ipAddress, int port, int bufferSize)
        {
            Name = name;
            IPAddress = ipAddress;
            Port = port;
            BufferSize = bufferSize;
        }
        
        public ConnectionSettings(string name, int port, int bufferSize)
        {
            Name = name;
            IPAddress = IPAddress.Any;
            Port = port;
            BufferSize = bufferSize;
        }
    }
}