using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using AppKit;
using CoreGraphics;
using Foundation;
using MultiChat.Common;
using MultiChat.Server.ClientTable;

namespace MultiChat.Server
{
    [Register("ServerController")]
    public partial class ServerController : NSViewController
    {
        private TcpListener Server { get; set; }
        private ServerStatus ServerStatus { get; set; }
        private List<Client> Clients { get; set; }
        private CancellationTokenSource ServerCancellationTokenSource { get; set; }
        private ConnectionSettings Settings { get; set; }

        public ServerController(IntPtr handle) : base(handle)
        {
            Clients = new List<Client>();
            ServerStatus = ServerStatus.Stopped;
        }

        public override async void ViewWillDisappear()
        {
            base.ViewWillDisappear();
            await SayGoodbye();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SendButton.Enabled = false;
            ChatMessageInput.Enabled = false;
            var dataSource = new ClientDataSource {Clients = this.Clients};
            ClientTable.DataSource = dataSource;
            ClientTable.Delegate = new ClientTableDelegate(dataSource);
        }

        async partial void StartButtonPressed(NSObject sender)
        {
            if (ServerStatus == ServerStatus.Stopped)
            {
                UpdateServerStatus(ServerStatus.Starting);
                Settings = new ConnectionSettings(NameInput.StringValue, PortInput.IntValue,
                    BufferSizeInput.IntValue);
                if (!Settings.Valid)
                {
                    DisplayError("One of the input values is incorrect!");
                    UpdateServerStatus(ServerStatus.Stopped);
                    return;
                }
                ServerCancellationTokenSource = new CancellationTokenSource();
                ServerCancellationTokenSource.Token.Register(Stop);
                Server = new TcpListener(Settings.IpAddress, Settings.Port);
                if (!Start()) return;
                UpdateServerStatus(ServerStatus.Started);
                await ListenAsync(ServerCancellationTokenSource.Token);
            }
            else if (ServerStatus == ServerStatus.Started)
            {
                UpdateServerStatus(ServerStatus.Stopping);
                await SayGoodbye();
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
                    var client = new Client(await Server.AcceptTcpClientAsync());
                    Clients.Add(client);
                    ReadAsync(client, token);
                }
                catch (Exception ex) when (ex is ObjectDisposedException || ex is SocketException)
                {
                    // This happens when the server is stopped
                    Console.WriteLine("Server stopped.");
                }
            }
        }

        private async void ReadAsync(Client client, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var stream = client.TcpClient.GetStream();
                    var message = new Message();
                    do
                    {
                        var buffer = new byte[Settings.BufferSize];
                        int bytesRead = await stream.ReadAsync(buffer, 0, Settings.BufferSize, token);
                        if (bytesRead > 0)
                        {
                            message.Append(buffer);
                        }
                    } while (!message.Terminated && stream.DataAvailable);

                    if (message.Empty) continue;
                    if (message.Special)
                    {
                        HandleSpecialMessage(client, message);
                    }
                    else
                    {
                        var decoded = message.Decode();
                        AppendMessage(decoded);
                        await BroadcastMessage(decoded, client);
                    }
                }
                catch (Exception ex) when (ex is InvalidOperationException || ex is ObjectDisposedException ||
                                           ex is IOException)
                {
                    // The client is no longer connected properly
                    DisconnectClient(client);
                    return;
                }
            }
        }

        private async Task WriteAsync(Client client, Message message)
        {
            try
            {
                var stream = client.TcpClient.GetStream();
                var bytes = message.Prepare();
                await stream.WriteAsync(bytes, 0, bytes.Length);
            }
            catch (Exception ex) when (ex is ObjectDisposedException || ex is IOException)
            {
                // Client disconnects during write
                Console.WriteLine($"Client {client.Name} disconnected during write.");
            }
        }

        async partial void SendButtonPressed(NSObject sender)
        {
            if (string.IsNullOrEmpty(ChatMessageInput.StringValue)) return;
            var message = $"{Settings.Name}: {ChatMessageInput.StringValue}";
            await BroadcastMessage(message, null);
            AppendMessage(message, NSColor.SystemBlueColor);
        }

        private async Task BroadcastMessage(string message, Client sender)
        {
            await BroadcastMessage(new Message(message), sender);
        }

        private async Task BroadcastMessage(Message message, Client sender)
        {
            IList<Task> queue = new List<Task>();
            foreach (var client in Clients)
            {
                if (client.Equals(sender)) continue;
                queue.Add(WriteAsync(client, message));
            }

            await Task.WhenAll(queue);
        }

        private bool Start()
        {
            try
            {
                Server.Start();
                return true;
            }
            catch (SocketException ex)
            {
                DisplayError($"Could not start server: {ex.Message}");
                UpdateServerStatus(ServerStatus.Stopped);
                return false;
            }
        }

        private void Stop()
        {
            Clients.Clear();
            ClientTable.ReloadData();
            Server.Stop();
        }

        private async Task SayGoodbye()
        {
            var farewell = new Message("bye", null);
            await BroadcastMessage(farewell, null);
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

        private void HandleSpecialMessage(Client initiator, Message message)
        {
            string[] values = message.GetValues();
            switch (values[0])
            {
                case "name":
                    initiator.Name = values.Length > 1 ? values[1] : "Anon";
                    ClientTable.ReloadData();
                    break;
                case "bye":
                    DisconnectClient(initiator);
                    break;
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
                    NameInput.Enabled = false;
                    PortInput.Enabled = false;
                    BufferSizeInput.Enabled = false;
                    BufferSizeSlider.Enabled = false;
                    break;
                case ServerStatus.Started:
                    StartButton.Enabled = true;
                    StartButton.Title = "Stop";
                    StatusIndicator.Image = NSImage.ImageNamed(NSImageName.StatusAvailable);
                    StatusIndicatorText.StringValue = "Server running";
                    SendButton.Enabled = true;
                    ChatMessageInput.Enabled = true;
                    AppendMessage($"~ Started listening on port {Settings.Port}.", NSColor.SystemGreenColor);
                    break;
                case ServerStatus.Stopping:
                    StartButton.Enabled = false;
                    StatusIndicator.Image = NSImage.ImageNamed(NSImageName.StatusPartiallyAvailable);
                    StatusIndicatorText.StringValue = "Stopping...";
                    SendButton.Enabled = false;
                    ChatMessageInput.Enabled = false;
                    break;
                case ServerStatus.Stopped:
                    StartButton.Enabled = true;
                    StartButton.Title = "Start";
                    StatusIndicator.Image = NSImage.ImageNamed(NSImageName.StatusUnavailable);
                    StatusIndicatorText.StringValue = "Server stopped";
                    NameInput.Enabled = true;
                    PortInput.Enabled = true;
                    BufferSizeInput.Enabled = true;
                    BufferSizeSlider.Enabled = true;
                    AppendMessage("~ Server stopped.", NSColor.SystemRedColor);
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

        private void DisconnectClient(Client client)
        {
            client.Dispose();
            Clients.Remove(client);
            ClientTable.ReloadData();
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