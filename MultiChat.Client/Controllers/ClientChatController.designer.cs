// WARNING
//
// This file has been generated automatically by Rider IDE
//   to store outlets and actions made in Xcode.
// If it is removed, they will be lost.
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

		[Action ("ClientChatSendButtonPressed:")]
		partial void ClientChatSendButtonPressed (AppKit.NSButton sender);

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
