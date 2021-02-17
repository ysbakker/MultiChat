using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MultiChat.Common.Services
{
    public class ListenerService
    {
        private readonly string Name;
        private readonly IPAddress IPAddress;
        private readonly int Port;
        private int BufferSize;
        private TcpListener Server;
        
        public ListenerService(string name, int port, int bufferSize)
        {
            Name = name;
            Port = port;
            BufferSize = bufferSize;
            IPAddress = IPAddress.Any;
        }

        public async Task Start()
        {
            Server = new TcpListener(IPAddress, Port);
            Server.Start();
            try
            {
                await Server.AcceptTcpClientAsync();
            }
            catch (SocketException e)
            {
                // TODO: Handle exception properly
                Console.WriteLine(e);
            }
        }
    }
}