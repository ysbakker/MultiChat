using System;
using AppKit;
using CoreGraphics;
using Foundation;

namespace MultiChat.Server.Controllers
{
    [Register("ServerChatController")]
    public partial class ServerChatController : NSViewController
    {
        public ServerSettingsController SettingsController { private get; set; }

        public ServerChatController(IntPtr handle) : base(handle)
        {
        }

        async partial void ServerChatSendButtonPressed(NSButton sender)
        {
            await SettingsController.Server.BroadcastMessage(SettingsController.Server.ServerClient,
                ServerChatMessageInput.StringValue, AppendMessage);
            ServerChatMessageInput.StringValue = string.Empty;
        }

        internal void AppendMessage(string message)
        {
            AppendMessage(message, NSColor.LabelColor);
        }

        internal void AppendMessage(string message, NSColor color)
        {
            var textView = (NSTextView) ServerChatMessageList.DocumentView;
            var msg = new NSAttributedString(message + "\n", foregroundColor: color);
            textView?.TextStorage.Append(msg);
            nfloat scrollPositionX = 0;
            nfloat scrollPositionY = ((NSView) ServerChatMessageList.DocumentView).Frame.Size.Height -
                                     ServerChatMessageList.ContentSize.Height;
            var scrollPoint = new CGPoint(scrollPositionX, scrollPositionY);
            ServerChatMessageList.ContentView.ScrollToPoint(scrollPoint);
        }
    }
}