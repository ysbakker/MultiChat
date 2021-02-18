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
        public ServerSettingsController(IntPtr handle) : base(handle)
        {
        }

        partial void ServerStartButtonPressed (NSButton sender)
        {
            var sw = Stopwatch.StartNew();
            var settings = new ConnectionSettingsModel(ServerName.StringValue, ServerPort.IntValue, ServerBufferSize.IntValue);
            var server = new Server(settings);
            server.Start();
            ChatController.AppendMessage($"* Server \"{ServerName.StringValue}\" started listening on port {ServerPort.IntValue}");
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
        }
    }
}