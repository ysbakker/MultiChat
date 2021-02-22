// WARNING
//
// This file has been generated automatically by Rider IDE
//   to store outlets and actions made in Xcode.
// If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace MultiChat.Server
{
	partial class ServerController
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
		AppKit.NSScrollView ClientList { get; set; }

		[Outlet]
		AppKit.NSTextField NameInput { get; set; }

		[Outlet]
		AppKit.NSTextField PortInput { get; set; }

		[Outlet]
		AppKit.NSButton StartButton { get; set; }

		[Outlet]
		AppKit.NSImageView StatusIndicator { get; set; }

		[Outlet]
		AppKit.NSTextField StatusIndicatorText { get; set; }

		[Action ("BufferSizeInputChanged:")]
		partial void BufferSizeInputChanged (Foundation.NSObject sender);

		[Action ("BufferSizeSliderChanged:")]
		partial void BufferSizeSliderChanged (Foundation.NSObject sender);

		[Action ("SendButtonPressed:")]
		partial void SendButtonPressed (Foundation.NSObject sender);

		[Action ("StartButtonPressed:")]
		partial void StartButtonPressed (Foundation.NSObject sender);

		void ReleaseDesignerOutlets ()
		{
			if (NameInput != null) {
				NameInput.Dispose ();
				NameInput = null;
			}

			if (PortInput != null) {
				PortInput.Dispose ();
				PortInput = null;
			}

			if (BufferSizeInput != null) {
				BufferSizeInput.Dispose ();
				BufferSizeInput = null;
			}

			if (BufferSizeSlider != null) {
				BufferSizeSlider.Dispose ();
				BufferSizeSlider = null;
			}

			if (StartButton != null) {
				StartButton.Dispose ();
				StartButton = null;
			}

			if (StatusIndicator != null) {
				StatusIndicator.Dispose ();
				StatusIndicator = null;
			}

			if (StatusIndicatorText != null) {
				StatusIndicatorText.Dispose ();
				StatusIndicatorText = null;
			}

			if (ClientList != null) {
				ClientList.Dispose ();
				ClientList = null;
			}

			if (ChatMessageList != null) {
				ChatMessageList.Dispose ();
				ChatMessageList = null;
			}

			if (ChatMessageInput != null) {
				ChatMessageInput.Dispose ();
				ChatMessageInput = null;
			}

		}
	}
}
