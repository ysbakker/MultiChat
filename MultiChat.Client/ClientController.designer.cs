// WARNING
//
// This file has been generated automatically by Rider IDE
//   to store outlets and actions made in Xcode.
// If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MultiChat.Client
{
	partial class ClientController
	{
		[Outlet]
		AppKit.NSTextField BufferSizeInput { get; set; }

		[Outlet]
		AppKit.NSSlider BufferSizeSlider { get; set; }

		[Outlet]
		AppKit.NSTextField ChatMessageInput { get; set; }

		[Outlet]
		AppKit.NSScrollView ChatMessageList { get; set; }

		[Outlet]
		AppKit.NSButton ConnectButton { get; set; }

		[Outlet]
		AppKit.NSTextField IPAddressInput { get; set; }

		[Outlet]
		AppKit.NSTextField NameInput { get; set; }

		[Outlet]
		AppKit.NSTextField PortInput { get; set; }

		[Outlet]
		AppKit.NSButton SendButton { get; set; }

		[Action ("BufferSizeInputChanged:")]
		partial void BufferSizeInputChanged (Foundation.NSObject sender);

		[Action ("BufferSizeSliderChanged:")]
		partial void BufferSizeSliderChanged (Foundation.NSObject sender);

		[Action ("ConnectButtonPressed:")]
		partial void ConnectButtonPressed (Foundation.NSObject sender);

		[Action ("SendButtonPressed:")]
		partial void SendButtonPressed (Foundation.NSObject sender);

		void ReleaseDesignerOutlets ()
		{
			if (BufferSizeInput != null) {
				BufferSizeInput.Dispose ();
				BufferSizeInput = null;
			}

			if (BufferSizeSlider != null) {
				BufferSizeSlider.Dispose ();
				BufferSizeSlider = null;
			}

			if (ChatMessageInput != null) {
				ChatMessageInput.Dispose ();
				ChatMessageInput = null;
			}

			if (ChatMessageList != null) {
				ChatMessageList.Dispose ();
				ChatMessageList = null;
			}

			if (ConnectButton != null) {
				ConnectButton.Dispose ();
				ConnectButton = null;
			}

			if (IPAddressInput != null) {
				IPAddressInput.Dispose ();
				IPAddressInput = null;
			}

			if (NameInput != null) {
				NameInput.Dispose ();
				NameInput = null;
			}

			if (PortInput != null) {
				PortInput.Dispose ();
				PortInput = null;
			}

			if (SendButton != null) {
				SendButton.Dispose ();
				SendButton = null;
			}

		}
	}
}
