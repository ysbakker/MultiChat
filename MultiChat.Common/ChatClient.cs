using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MultiChat.Common
{
    public class ChatClient : IDisposable
    {
        private readonly TcpClient _client;

        public ChatClient(TcpClient client)
        {
            _client = client;
        }

        public async void ReadAsync(Action<string> messageHandler, int bufferSize)
        {
            var buffer = new byte[bufferSize];
            await using var stream = _client.GetStream();
            while (true)
            {
                var readBytes = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (readBytes == 0) continue;
                var message = Encoding.Unicode.GetString(buffer);
                messageHandler(message);
            }
        }

        public async Task WriteAsync(string message)
        {
            var bytes = Encoding.Unicode.GetBytes(message);
            await using var stream = _client.GetStream();
            await stream.WriteAsync(bytes, 0, bytes.Length);
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}