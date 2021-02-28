using System;
using System.Net;

namespace MultiChat.Common
{
    public class ConnectionSettings
    {
        public Validatable<string> Name { get; }
        public Validatable<IPAddress> IPAddress { get; }
        public Validatable<int> Port { get; }
        public Validatable<int> BufferSize { get; }

        private readonly Predicate<string> _nameValidator =
            (name) => !string.IsNullOrEmpty(name) && name.Length > 1 && name.Length < 20;
        private readonly Predicate<IPAddress> _ipValidator =
            (ip) => ip != null;
        private readonly Predicate<int> _portValidator =
            (port) => port >= 1024 && port <= 65535;
        private readonly Predicate<int> _bufferSizeValidator =
            (bufferSize) => bufferSize >= 1 && bufferSize <= 65535;
        
        public bool Valid => Name.Valid() && IPAddress.Valid() && Port.Valid() && BufferSize.Valid();

        public ConnectionSettings(string name, IPAddress ipAddress, int port, int bufferSize)
        {
            Name = new Validatable<string>(name, _nameValidator);
            IPAddress = new Validatable<IPAddress>(ipAddress, _ipValidator);
            Port = new Validatable<int>(port, _portValidator);
            BufferSize = new Validatable<int>(bufferSize, _bufferSizeValidator);
        }

        public ConnectionSettings(string name, int port, int bufferSize)
        {
            Name = new Validatable<string>(name, _nameValidator);
            IPAddress = new Validatable<IPAddress>(System.Net.IPAddress.Any, _ipValidator);
            Port = new Validatable<int>(port, _portValidator);
            BufferSize = new Validatable<int>(bufferSize, _bufferSizeValidator);
        }
    }
}