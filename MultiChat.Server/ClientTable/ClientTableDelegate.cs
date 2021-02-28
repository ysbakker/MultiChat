using System;
using AppKit;

namespace MultiChat.Server.ClientTable
{
    public class ClientTableDelegate : NSTableViewDelegate
    {
        private const string CellIdentifier = "ClientCell";
        private ClientDataSource _dataSource;

        public ClientTableDelegate(ClientDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public override NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
        {
            NSTextField view = (NSTextField) tableView.MakeView(CellIdentifier, this);
            if (view == null) {
                view = new NSTextField ();
                view.Identifier = CellIdentifier;
                view.BackgroundColor = NSColor.Clear;
                view.Bordered = false;
                view.Selectable = false;
                view.Editable = false;
            }

            view.StringValue = _dataSource.Clients[(int) row].Name;
            
            return view;
        }
    }
}