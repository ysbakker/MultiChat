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
	[Register ("ServerSettingsController")]
	partial class ServerSettingsController
	{
		[Outlet]
		AppKit.NSTextField ServerBufferSize { get; set; }

		[Outlet]
		AppKit.NSSlider ServerBufferSizeSlider { get; set; }

		[Outlet]
		AppKit.NSOutlineView ServerClientList { get; set; }

		[Outlet]
		AppKit.NSTextField ServerName { get; set; }

		[Outlet]
		AppKit.NSTextField ServerPort { get; set; }

		[Outlet]
		AppKit.NSButton ServerStartButton { get; set; }

		[Action ("ServerBufferSizeChanged:")]
		partial void ServerBufferSizeChanged (AppKit.NSTextField sender);

		[Action ("ServerBufferSizeSliderChanged:")]
		partial void ServerBufferSizeSliderChanged (AppKit.NSSlider sender);

		[Action ("ServerStartButtonPressed:")]
		partial void ServerStartButtonPressed (AppKit.NSButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (ServerBufferSize != null) {
				ServerBufferSize.Dispose ();
				ServerBufferSize = null;
			}

			if (ServerBufferSizeSlider != null) {
				ServerBufferSizeSlider.Dispose ();
				ServerBufferSizeSlider = null;
			}

			if (ServerClientList != null) {
				ServerClientList.Dispose ();
				ServerClientList = null;
			}

			if (ServerName != null) {
				ServerName.Dispose ();
				ServerName = null;
			}

			if (ServerPort != null) {
				ServerPort.Dispose ();
				ServerPort = null;
			}

			if (ServerStartButton != null) {
				ServerStartButton.Dispose ();
				ServerStartButton = null;
			}

		}
	}
}
