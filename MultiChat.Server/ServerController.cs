using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AppKit;
using CoreGraphics;
using Foundation;

namespace MultiChat.Server
{
    [Register("ServerController")]
    public partial class ServerController : NSViewController
    {
        private TcpListener Server { get; set; }
        private ServerStatus ServerStatus { get; set; }
        private ObservableCollection<TcpClient> Clients { get; set; }
        private CancellationTokenSource ServerCancellationTokenSource { get; set; }

        public ServerController(IntPtr handle) : base(handle)
        {
            Clients = new ObservableCollection<TcpClient>();
            Clients.CollectionChanged += UpdateClientList;
            ServerStatus = ServerStatus.Stopped;
        }

        async partial void StartButtonPressed(NSObject sender)
        {
            if (ServerStatus == ServerStatus.Stopped)
            {
                UpdateServerStatus(ServerStatus.Starting);
                ServerCancellationTokenSource = new CancellationTokenSource();
                ServerCancellationTokenSource.Token.Register(() => { Server.Stop(); });
                Server = new TcpListener(IPAddress.Any, PortInput.IntValue);
                Server.Start();
                UpdateServerStatus(ServerStatus.Started);
                await ListenAsync(ServerCancellationTokenSource.Token);
            }
            else if (ServerStatus == ServerStatus.Started)
            {
                UpdateServerStatus(ServerStatus.Stopping);
                ServerCancellationTokenSource.Cancel();
                UpdateServerStatus(ServerStatus.Stopped);
            }
        }

        private async Task ListenAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var client = await Server.AcceptTcpClientAsync();
                    Clients.Add(client);
                    ReadAsync(client, token);
                }
                catch (ObjectDisposedException)
                {
                    // TODO: Better exception handling
                    // This happens when the server is stopped
                    Console.WriteLine("TcpListener was disposed");
                }
                catch (SocketException)
                {
                    // TODO: Better exception handling
                    Console.WriteLine("Something went wrong in the socket");
                }
                catch (InvalidOperationException)
                {
                    // TODO: Better exception handling
                    Console.WriteLine("TcpClient not connected");
                }
                catch (IOException)
                {
                    // TODO: Better exception handling
                    Console.WriteLine("NetworkStream connection failure");
                }
            }
        }

        private async void ReadAsync(TcpClient client, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var bufferSize = BufferSizeInput.IntValue;
                    var stream = client.GetStream();
                    var message = new StringBuilder();
                    do
                    {
                        var buffer = new byte[bufferSize];
                        int bytesRead = await stream.ReadAsync(buffer, 0, bufferSize, token);
                        if (bytesRead > 0)
                        {
                            message.Append(Encoding.Unicode.GetString(buffer));
                        }
                    } while (stream.DataAvailable);

                    if (string.IsNullOrEmpty(message.ToString())) continue;
                    AppendMessage(message.ToString());
                    await BroadcastMessage(message.ToString(), client);
                }
                catch (Exception ex) when (ex is InvalidOperationException || ex is ObjectDisposedException || ex is IOException)
                {
                    // TODO
                    // The client is no longer connected properly
                    DisplayError("Client lost connection.");
                    client.Dispose();
                    Clients.Remove(client);
                    return;
                }
            }
        }

        private async Task WriteAsync(TcpClient client, string message)
        {
            try
            {
                var stream = client.GetStream();
                var bytes = Encoding.Unicode.GetBytes(message);
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

        async partial void SendButtonPressed(NSObject sender)
        {
            var message = $"Server: {ChatMessageInput.StringValue}";
            await BroadcastMessage(message, null);
            AppendMessage(message, NSColor.SystemBlueColor);
        }

        private async Task BroadcastMessage(string message, TcpClient sender)
        {
            IList<Task> queue = new List<Task>();
            foreach (var client in Clients)
            {
                if (client.Equals(sender)) continue;
                queue.Add(WriteAsync(client, message));
            }

            await Task.WhenAll(queue);
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

        private void UpdateClientList(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems?.Count > 0)
            {
                AppendMessage($"{e.OldItems[0].GetHashCode()} disconnected.");
            }
            else if (e.NewItems?.Count > 0)
            {
                AppendMessage($"{e.NewItems[0].GetHashCode()} connected.");
            }
        }

        private void UpdateServerStatus(ServerStatus status)
        {
            ServerStatus = status;
            switch (status)
            {
                case ServerStatus.Starting:
                    StartButton.Enabled = false;
                    StatusIndicator.Image = NSImage.ImageNamed(NSImageName.StatusPartiallyAvailable);
                    StatusIndicatorText.StringValue = "Starting...";
                    break;
                case ServerStatus.Started:
                    StartButton.Enabled = true;
                    StartButton.Title = "Stop";
                    AppendMessage($"~ Started listening on port {PortInput.StringValue}.", NSColor.SystemGreenColor);
                    StatusIndicator.Image = NSImage.ImageNamed(NSImageName.StatusAvailable);
                    StatusIndicatorText.StringValue = "Server running";
                    break;
                case ServerStatus.Stopping:
                    StartButton.Enabled = false;
                    StatusIndicator.Image = NSImage.ImageNamed(NSImageName.StatusPartiallyAvailable);
                    StatusIndicatorText.StringValue = "Stopping...";
                    break;
                case ServerStatus.Stopped:
                    StartButton.Enabled = true;
                    StartButton.Title = "Start";
                    AppendMessage("~ Server stopped.", NSColor.SystemRedColor);
                    StatusIndicator.Image = NSImage.ImageNamed(NSImageName.StatusUnavailable);
                    StatusIndicatorText.StringValue = "Server stopped";
                    break;
                default:
                    return;
            }
        }

        private void DisplayError(string message)
        {
            var alert = new NSAlert()
            {
                AlertStyle = NSAlertStyle.Critical,
                InformativeText = message ?? "Something went wrong.",
                MessageText = "Error occured"
            };
            alert.RunModal();
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