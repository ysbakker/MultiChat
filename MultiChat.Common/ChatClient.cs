using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using MultiChat.Common.Models;

namespace MultiChat.Common
{
    public class ChatClient : IDisposable
    {
        private string Name { get; set; }
        private int BufferSize { get; set; }
        private TcpClient Client { get; set; }

        public ChatClient(TcpClient client)
        {
            Client = client;
        }

        public async Task ConnectAsync(ConnectionSettingsModel connectionSettings)
        {
            Name = connectionSettings.Name;
            BufferSize = connectionSettings.BufferSize;
            await Client.ConnectAsync(connectionSettings.IPAddress, connectionSettings.Port);
        }

        public async void ReadAsync(Func<string, Task> messageHandler, int bufferSize)
        {
            var buffer = new byte[bufferSize];
            await using var stream = Client.GetStream();
            while (true)
            {
                var readBytes = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (readBytes == 0) continue;
                var message = Encoding.Unicode.GetString(buffer);
                await messageHandler(message);
            }
        }

        public async Task WriteAsync(string message)
        {
            var bytes = Encoding.Unicode.GetBytes(message);
            await using var stream = Client.GetStream();
            await stream.WriteAsync(bytes, 0, bytes.Length);
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
}