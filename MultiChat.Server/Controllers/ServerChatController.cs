using System;
using System.Threading.Tasks;
using AppKit;
using CoreGraphics;
using CoreText;
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
            var textView = (NSTextView) ServerChatMessageList.DocumentView;
            var msg = new NSAttributedString(message + "\n", foregroundColor: NSColor.LabelColor);
            textView?.TextStorage.Append(msg);
        }

        internal void AppendMessage(string message, NSColor color)
        {
            var textView = (NSTextView) ServerChatMessageList.DocumentView;
            var msg = new NSAttributedString(message + "\n", foregroundColor: color);
            textView?.TextStorage.Append(msg);
        }
    }
}