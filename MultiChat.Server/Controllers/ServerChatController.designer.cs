// WARNING
//
// This file has been generated automatically by Rider IDE
//   to store outlets and actions made in Xcode.
// If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MultiChat.Server.Controllers
{
	partial class ServerChatController
	{
		[Outlet]
		AppKit.NSTextField ServerChatMessageInput { get; set; }

		[Outlet]
		AppKit.NSScrollView ServerChatMessageList { get; set; }

		[Action ("ServerChatSendButtonPressed:")]
		partial void ServerChatSendButtonPressed (AppKit.NSButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (ServerChatMessageList != null) {
				ServerChatMessageList.Dispose ();
				ServerChatMessageList = null;
			}

			if (ServerChatMessageInput != null) {
				ServerChatMessageInput.Dispose ();
				ServerChatMessageInput = null;
			}

		}
	}
}
