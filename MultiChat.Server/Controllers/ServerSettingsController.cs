using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AppKit;
using CoreText;
using Foundation;
using MultiChat.Common.Models;

namespace MultiChat.Server.Controllers
{
    public partial class ServerSettingsController : NSViewController
    {
        public ServerChatController ChatController { private get; set; }
        private Server Server { get; set; }
        private CancellationTokenSource ServerCancellationTokenSource { get; set; }

        public ServerSettingsController(IntPtr handle) : base(handle)
        {
            Server = new Server();
        }

        async partial void ServerStartButtonPressed(NSButton sender)
        {
            var settings =
                new ConnectionSettingsModel(ServerName.StringValue, ServerPort.IntValue, ServerBufferSize.IntValue);

            if (!Server.Running)
            {
                ServerStartButton.Enabled = false;
                ServerCancellationTokenSource = new CancellationTokenSource();
                Server.Start(settings);
                
                ChatController.AppendMessage(
                    $"* Server \"{ServerName.StringValue}\" started listening on port {ServerPort.IntValue}.",
                    NSColor.Green);
                ServerStartButton.Title = "Stop";
                ServerStartButton.Enabled = true;
                
                await Server.Listen(ServerCancellationTokenSource.Token);
            }
            else
            {
                Server.Stop();
                ServerCancellationTokenSource.Cancel();
                ChatController.AppendMessage($"* Server \"{ServerName.StringValue}\" stopped.", NSColor.Red);
                ServerStartButton.Title = "Start";
                ServerCancellationTokenSource.Dispose();
            }
        }
    }
}