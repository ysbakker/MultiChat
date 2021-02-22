using System;

using AppKit;
using Foundation;

namespace MultiChat.Client.Controllers
{
    [Register("ClientSplitController")]
    public partial class ClientSplitController : NSSplitViewController
    {
        private ClientSettingsController SettingsController { get; set; }
        private ClientChatController ChatController { get; set; }
        
        public ClientSplitController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SettingsController = (ClientSettingsController)SplitViewItems[0].ViewController;
            ChatController = (ClientChatController)SplitViewItems[1].ViewController;
            SettingsController.ChatController = ChatController;
            ChatController.SettingsController = SettingsController;
        }
    }
}