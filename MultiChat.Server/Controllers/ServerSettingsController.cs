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

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ServerStatusImage.Image = NSImage.ImageNamed(NSImageName.StatusUnavailable);
            ServerStatusText.StringValue = "Server stopped";
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
                
                ServerStartButton.Enabled = true;
                ChatController.AppendMessage(
                    $"~ Server \"{ServerName.StringValue}\" started listening on port {ServerPort.IntValue}.",
                    NSColor.Green);
                ServerStartButton.Title = "Stop";
                ServerStatusImage.Image = NSImage.ImageNamed(NSImageName.StatusAvailable);
                ServerStatusText.StringValue = "Server running";
                
                await Server.Listen(ServerCancellationTokenSource.Token);
            }
            else
            {
                ServerStartButton.Enabled = false;
                
                Server.Stop();
                ServerCancellationTokenSource.Cancel();
                ServerCancellationTokenSource.Dispose();
                
                ServerStartButton.Enabled = true;
                ChatController.AppendMessage($"~ Server \"{ServerName.StringValue}\" stopped.", NSColor.Red);
                ServerStartButton.Title = "Start";
                ServerStatusImage.Image = NSImage.ImageNamed(NSImageName.StatusUnavailable);
                ServerStatusText.StringValue = "Server stopped";
            }
        }

        partial void ServerBufferSizeSliderChanged(NSSlider sender)
        {
            ServerBufferSize.IntValue = sender.IntValue;
        }

        partial void ServerBufferSizeChanged(NSTextField sender)
        {
            ServerBufferSizeSlider.IntValue = sender.IntValue;
        }

    }
}