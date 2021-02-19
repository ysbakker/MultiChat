using System;
using AppKit;

namespace MultiChat.Client.Controllers
{
    public partial class ClientSettingsController : NSViewController
    {
        public ClientSettingsController(IntPtr handle) : base(handle)
        {
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