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
using MultiChat.Common;

namespace MultiChat.Client
{
    [Register("ClientController")]
    public partial class ClientController : NSViewController
    {
        private ClientStatus ClientStatus { get; set; }
        private TcpClient Client { get; set; }
        private string ClientName { get; set; }
        private CancellationTokenSource ClientCancellationTokenSource { get; set; }

        public ClientController(IntPtr handle) : base(handle)
        {
            ClientStatus = ClientStatus.Disconnected;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SendButton.Enabled = false;
            ChatMessageInput.Enabled = false;
        }

        async partial void ConnectButtonPressed(NSObject sender)
        {
            if (ClientStatus == ClientStatus.Disconnected)
            {
                UpdateClientStatus(ClientStatus.Connecting);
                ClientName = NameInput.StringValue;
                ClientCancellationTokenSource = new CancellationTokenSource();
                ClientCancellationTokenSource.Token.Register(() => { Client.Dispose(); });
                Client = new TcpClient();
                await Client.ConnectAsync(IPAddress.Parse(IPAddressInput.StringValue), PortInput.IntValue);
                await AnnounceName();
                UpdateClientStatus(ClientStatus.Connected);
                await ReadAsync(ClientCancellationTokenSource.Token);
            }
            else if (ClientStatus == ClientStatus.Connected)
            {
                UpdateClientStatus(ClientStatus.Disconnecting);
                ClientCancellationTokenSource.Cancel();
                UpdateClientStatus(ClientStatus.Disconnected);
            }
        }

        async partial void SendButtonPressed(NSObject sender)
        {
            var message = $"{ClientName}: {ChatMessageInput.StringValue}";
            await WriteAsync(message);
            AppendMessage(message, NSColor.SystemBlueColor);
        }

        private async Task ReadAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var bufferSize = BufferSizeInput.IntValue;
                    var stream = Client.GetStream();
                    var message = new Message();
                    do
                    {
                        var buffer = new byte[bufferSize];
                        int bytesRead = await stream.ReadAsync(buffer, 0, bufferSize, token);
                        if (bytesRead > 0)
                        {
                            message.Append(buffer);
                        }
                    } while (!message.Terminated && stream.DataAvailable);

                    if (message.Empty) continue;
                    var decoded = message.Decode();
                    AppendMessage(decoded);
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
            await WriteAsync(new Message(message));
        }

        private async Task WriteAsync(Message message)
        {
            try
            {
                var bytes = message.Prepare();
                var stream = Client.GetStream();
                await stream.WriteAsync(bytes, 0, bytes.Length);
            }
            catch (ObjectDisposedException)
            {
                // TODO: Better exception handling
                Console.WriteLine("Couldn't write because object was disposed");
            }
            catch (IOException)
            {
                // TODO: Better exception handling
                Console.WriteLine("NetworkStream connection failure");
            }
        }

        private async Task AnnounceName()
        {
            var message = new Message("name", ClientName);
            await WriteAsync(message);
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

        private void UpdateClientStatus(ClientStatus status)
        {
            ClientStatus = status;
            switch (status)
            {
                case ClientStatus.Connecting:
                    ConnectButton.Enabled = false;
                    NameInput.Enabled = false;
                    IPAddressInput.Enabled = false;
                    PortInput.Enabled = false;
                    BufferSizeInput.Enabled = false;
                    BufferSizeSlider.Enabled = false;
                    AppendMessage("~ Connecting to server...", NSColor.SystemYellowColor);
                    break;
                case ClientStatus.Connected:
                    ConnectButton.Enabled = true;
                    ConnectButton.Title = "Disconnect";
                    SendButton.Enabled = true;
                    ChatMessageInput.Enabled = true;
                    AppendMessage($"~ Connected to {IPAddressInput.StringValue}.", NSColor.SystemGreenColor);
                    break;
                case ClientStatus.Disconnecting:
                    ConnectButton.Enabled = false;
                    SendButton.Enabled = false;
                    ChatMessageInput.Enabled = false;
                    break;
                case ClientStatus.Disconnected:
                    ConnectButton.Enabled = true;
                    ConnectButton.Title = "Connect";
                    NameInput.Enabled = true;
                    PortInput.Enabled = true;
                    IPAddressInput.Enabled = true;
                    BufferSizeInput.Enabled = true;
                    BufferSizeSlider.Enabled = true;
                    AppendMessage("~ Disconnected.", NSColor.SystemRedColor);
                    break;
                default:
                    return;
            }
        }

        partial void BufferSizeSliderChanged(NSObject sender)
        {
            BufferSizeInput.IntValue = BufferSizeSlider.IntValue;
        }

        partial void BufferSizeInputChanged(NSObject sender)
        {
            BufferSizeSlider.IntValue = BufferSizeInput.IntValue;
        }
    }
}