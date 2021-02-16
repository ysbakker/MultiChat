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
	[Register ("ClientSettingsController")]
	partial class ClientSettingsController
	{
		[Outlet]
		AppKit.NSTextField ClientBufferSize { get; set; }

		[Outlet]
		AppKit.NSSlider ClientBufferSizeSlider { get; set; }

		[Outlet]
		AppKit.NSTextField ClientIP { get; set; }

		[Outlet]
		AppKit.NSTextField ClientName { get; set; }

		[Outlet]
		AppKit.NSTextField ClientPort { get; set; }

		[Action ("ClientBufferSizeSliderDidChange:")]
		partial void ClientBufferSizeSliderDidChange (AppKit.NSSlider sender);

		void ReleaseDesignerOutlets ()
		{
			if (ClientBufferSize != null) {
				ClientBufferSize.Dispose ();
				ClientBufferSize = null;
			}

			if (ClientBufferSizeSlider != null) {
				ClientBufferSizeSlider.Dispose ();
				ClientBufferSizeSlider = null;
			}

			if (ClientIP != null) {
				ClientIP.Dispose ();
				ClientIP = null;
			}

			if (ClientName != null) {
				ClientName.Dispose ();
				ClientName = null;
			}

			if (ClientPort != null) {
				ClientPort.Dispose ();
				ClientPort = null;
			}

		}
	}
}
