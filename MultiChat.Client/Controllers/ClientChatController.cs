using System;
using AppKit;

namespace MultiChat.Client.Controllers
{
    public partial class ClientChatController : NSViewController
    {
        public ClientSettingsController SettingsController { private get; set; }
        public ClientChatController(IntPtr handle) : base(handle)
        {
        }
    }
}