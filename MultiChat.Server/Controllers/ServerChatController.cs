using System;

using AppKit;
using Foundation;

namespace MultiChat.Server.Controllers
{
    [Register("ServerChatController")]
    public partial class ServerChatController : NSViewController
    {
        public ServerChatController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Do any additional setup after loading the view.
        }
    }
}