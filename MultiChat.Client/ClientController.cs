using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AppKit;
using CoreGraphics;
using Foundation;

namespace MultiChat.Client
{
    [Register("ClientController")]
    public partial class ClientController : NSViewController
    {
        private TcpClient Client { get; set; }
        private string ClientName { get; set; }
        private CancellationTokenSource ClientCancellationTokenSource { get; set; }
        
        public ClientController(IntPtr handle) : base(handle)
        {
        }

        async partial void ConnectButtonPressed(NSObject sender)
        {
            ClientName = NameInput.StringValue;
            ClientCancellationTokenSource = new CancellationTokenSource();
            Client = new TcpClient();
            await Client.ConnectAsync(IPAddress.Parse(IPAddressInput.StringValue), PortInput.IntValue);
            await ReadAsync(ClientCancellationTokenSource.Token);
        }

        partial void BufferSizeSliderChanged(NSObject sender)
        {
            BufferSizeInput.IntValue = BufferSizeSlider.IntValue;
        }

        partial void BufferSizeInputChanged(NSObject sender)
        {
            BufferSizeSlider.IntValue = BufferSizeInput.IntValue;
        }

        async partial void SendButtonPressed(NSObject sender)
        {
            await WriteAsync(ChatMessageInput.StringValue);
        }

        private async Task ReadAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var bufferSize = 1024;
                try
                {
                    var stream = Client.GetStream();
                    StringBuilder message = new StringBuilder();
                    do
                    {
                        var buffer = new byte[bufferSize];
                        await stream.ReadAsync(buffer, 0, bufferSize, token);
                        message.Append(Encoding.Unicode.GetString(buffer));
                    } while (stream.DataAvailable);

                    AppendMessage(message.ToString());
                }
                catch (ObjectDisposedException)
                {
                    // TODO: Better exception handling
                    Console.WriteLine("Stopped reading because object was disposed");
                }
                catch (IOException)
                {
                    // TODO: Better exception handling
                    Console.WriteLine("NetworkStream connection failure");
                }
            }
        }
        private async Task WriteAsync(string message)
        {
            var stream = Client.GetStream();
            var bytes = Encoding.Unicode.GetBytes($"{ClientName}: {message}");
            await stream.WriteAsync(bytes, 0, bytes.Length);
        }
        
        private void AppendMessage(string message)
        {
            AppendMessage(message, NSColor.LabelColor);
        }

        private void AppendMessage(string message, NSColor color)
        {
            var textView = (NSTextView) ChatMessageList.DocumentView;
            var msg = new NSAttributedString(message + "\n", foregroundColor: color);
            textView?.TextStorage.Append(msg);
            nfloat scrollPositionX = 0;
            nfloat scrollPositionY = ((NSView) ChatMessageList.DocumentView).Frame.Size.Height -
                                     ChatMessageList.ContentSize.Height;
            var scrollPoint = new CGPoint(scrollPositionX, scrollPositionY);
            ChatMessageList.ContentView.ScrollToPoint(scrollPoint);
        }
        
    }
}