using System;
using System.Net;
using System.Net.Sockets;
using AppKit;
using MultiChat.Common;
using MultiChat.Common.Models;

namespace MultiChat.Client.Controllers
{
    public partial class ClientSettingsController : NSViewController
    {
        private ChatClient Client { get; set; }

        public ClientSettingsController(IntPtr handle) : base(handle)
        {
        }

        async partial void ClientConnectButtonPressed(NSButton sender)
        {
            ConnectionSettingsModel connectionSettings = new ConnectionSettingsModel(ClientName.StringValue,
                IPAddress.Parse(ClientIP.StringValue), ClientPort.IntValue, ClientBufferSize.IntValue);
            Client = new ChatClient(new TcpClient());
            try
            {
                await Client.ConnectAsync(connectionSettings);
                await Client.WriteAsync("Hello!");
            }
            catch (SocketException)
            {
                // TODO: Better exception handling
                Console.WriteLine("Could not connect :(");
            }
        }

        partial void ClientBufferSizeSliderChanged(NSSlider sender)
        {
            ClientBufferSize.IntValue = sender.IntValue;
        }

        partial void ClientBufferSizeChanged(NSTextField sender)
        {
            ClientBufferSizeSlider.IntValue = sender.IntValue;
        }
    }
}