using System;

using AppKit;
using Foundation;

namespace MultiChat.Server.Controllers
{
    [Register("ServerSplitController")]
    public partial class ServerSplitController : NSSplitViewController
    {
        private ServerSettingsController _settingsController { get; set; }
        private ServerChatController _chatController { get; set; } 
        
        public ServerSplitController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            _settingsController = (ServerSettingsController)SplitViewItems[0].ViewController;
            _chatController = (ServerChatController)SplitViewItems[1].ViewController;
            _settingsController.ChatController = _chatController;
            _chatController.SettingsController = _settingsController;
        }
    }
}