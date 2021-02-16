// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MultiChat.Client.Controllers
{
	[Register ("ClientChatController")]
	partial class ClientChatController
	{
		[Outlet]
		AppKit.NSTextField ClientChatMessageBox { get; set; }

		[Outlet]
		AppKit.NSScrollView ClientChatMessageList { get; set; }

		[Action ("ClientChatSendButton:")]
		partial void ClientChatSendButton (AppKit.NSButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ClientChatMessageBox != null) {
				ClientChatMessageBox.Dispose ();
				ClientChatMessageBox = null;
			}

			if (ClientChatMessageList != null) {
				ClientChatMessageList.Dispose ();
				ClientChatMessageList = null;
			}
		}
	}
}
