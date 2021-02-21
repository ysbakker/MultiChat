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
        public bool Running { get; private set; }
        private ConnectionSettingsModel ConnectionSettings { get; set; }
        private TcpListener Listener { get; set; }
        private IList<ChatClient> Clients { get; set; }
        private ChatClient ServerClient { get; set; }
        private Action<string> ReceiveMessageHandler { get; set; }

        public Server(Action<string> receiveMessageHandler)
        {
            Clients = new List<ChatClient>();
            ServerClient = new ChatClient(new TcpClient());
            ReceiveMessageHandler = receiveMessageHandler;
            Running = false;
        }

        public void Start(ConnectionSettingsModel settings)
        {
            ConnectionSettings = settings;
            Listener = new TcpListener(settings.IPAddress, settings.Port);
            Listener.Start();
            Running = true;
        }

        public async Task Listen(CancellationToken token)
        {
            token.Register(() => Listener.Stop());
            await AcceptConnections(token);
        }

        public void Stop()
        {
            Running = false;
        }

        private async Task AcceptConnections(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var tcpClient = await Listener.AcceptTcpClientAsync();
                    var chatClient = new ChatClient(tcpClient);
                    Clients.Add(chatClient);
                    chatClient.ReadAsync(async message => { await BroadcastMessage(chatClient, message); }, 1024);
                }
                catch (SocketException)
                {
                    Console.WriteLine("Oopsie listener was closed :(");
                }
                catch (ObjectDisposedException)
                {
                    Console.WriteLine("Oopsie object was disposed :(");
                }
            }
        }

        public async Task BroadcastMessage(ChatClient initiator, string message)
        {
            if (initiator.Equals(ServerClient))
            {
                ReceiveMessageHandler($"<< {message}");
            }
            else
            {
                ReceiveMessageHandler($">> {message}");
            }

            IList<Task> sendQueue = new List<Task>();
            foreach (ChatClient client in Clients)
            {
                sendQueue.Add(client.WriteAsync(message));
            }

            await Task.WhenAll(sendQueue);
        }
    }
}