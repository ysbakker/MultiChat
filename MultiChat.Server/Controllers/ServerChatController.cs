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

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Do any additional setup after loading the view.
        }
    }
}