using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using MultiChat.Common;
using MultiChat.Common.Models;

namespace MultiChat.Server
{
    public class Server
    {
        private readonly ConnectionSettingsModel _connectionSettings;
        private TcpListener _listener;
        private IList<ChatClient> _clients;
        
        public Server(ConnectionSettingsModel connectionSettings)
        {
            _connectionSettings = connectionSettings;
            _clients = new List<ChatClient>();
        }

        public void Start()
        {
            _listener = new TcpListener(_connectionSettings.IPAddress, _connectionSettings.Port);
            _listener.Start();
            AcceptConnections();
        }

        private async void AcceptConnections()
        {
            Action<string> messageHandler = async message =>
            {
                var outgoingMessages = new List<Task>();
                foreach (ChatClient client in _clients)
                {
                    outgoingMessages.Add(client.WriteAsync(message));
                }
                await Task.WhenAll(outgoingMessages);
            };
            while (true)
            {
                var tcpClient = await _listener.AcceptTcpClientAsync();
                var chatClient = new ChatClient(tcpClient);
                chatClient.ReadAsync(messageHandler, 100);
                _clients.Add(chatClient);
            }
        }
    }
}