using System;
using AppKit;
using CoreGraphics;
using Foundation;

namespace MultiChat.Server.Controllers
{
    [Register("ServerChatController")]
    public partial class ServerChatController : NSViewController
    {
        public ServerChatController(IntPtr handle) : base(handle)
        {
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
            nfloat scrollPostitionX = 0;
            nfloat scrollPositionY = ((NSView) ServerChatMessageList.DocumentView).Frame.Size.Height -
                                     ServerChatMessageList.ContentSize.Height;
            var scrollPoint = new CGPoint(scrollPostitionX, scrollPositionY);
            ServerChatMessageList.ContentView.ScrollToPoint(scrollPoint);
        }
    }
}