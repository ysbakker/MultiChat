using System;

using AppKit;
using Foundation;

namespace MultiChat.Server.Controllers
{
    [Register("ServerSplitController")]
    public partial class ServerSplitController : NSSplitViewController
    {
        private ServerSettingsController SettingsController { get; set; }
        private ServerChatController ChatController { get; set; } 
        
        public ServerSplitController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SettingsController = (ServerSettingsController)SplitViewItems[0].ViewController;
            ChatController = (ServerChatController)SplitViewItems[1].ViewController;
            SettingsController.ChatController = ChatController;
            ChatController.SettingsController = SettingsController;
        }
    }
}