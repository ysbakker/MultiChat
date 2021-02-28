using System;
using System.Collections.Generic;
using AppKit;

namespace MultiChat.Server.ClientTable
{
    public class ClientDataSource : NSTableViewDataSource
    {
        public List<Client> Clients { get; set; }

        public ClientDataSource()
        {
            Clients = new List<Client>();
        }

        public override nint GetRowCount(NSTableView tableView)
        {
            return Clients.Count;
        }
    }
}