using System;
using System.Net.Sockets;

namespace MultiChat.Server
{
    public class Client : IDisposable
    {
        public TcpClient TcpClient { get; }

        private string _name;
        public string Name
        {
            get => _name ?? GetHashCode().ToString();
            set => _name = value;
        }

        public Client(TcpClient tcpClient)
        {
            TcpClient = tcpClient;
        }

        public void Dispose()
        {
            TcpClient?.Dispose();
        }
    }
}