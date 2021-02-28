using System;
using System.Net;

namespace MultiChat.Common
{
    public class ConnectionSettings
    {
        private Validatable<string> NameValidatable { get; }
        private Validatable<string> IpAddressValidatable { get; }
        private Validatable<int> PortValidatable { get; }
        private Validatable<int> BufferSizeValidatable { get; }
        public string Name => NameValidatable.Value;
        public IPAddress IpAddress => IpAddressValidatable.Valid() ? IPAddress.Parse(IpAddressValidatable.Value) : null;
        public int Port => PortValidatable.Value;
        public int BufferSize => BufferSizeValidatable.Value;

        public bool Valid => NameValidatable.Valid() && IpAddressValidatable.Valid() && PortValidatable.Valid() &&
                             BufferSizeValidatable.Valid();

        private readonly Predicate<string> _nameValidator =
            (name) => !string.IsNullOrEmpty(name) && name.Length > 1 && name.Length < 20;

        private readonly Predicate<string> _ipValidator =
            (ip) => IPAddress.TryParse(ip, out _);

        private readonly Predicate<int> _portValidator =
            (port) => port >= 1024 && port <= 65535;

        private readonly Predicate<int> _bufferSizeValidator =
            (bufferSize) => bufferSize >= 1 && bufferSize <= 65535;


        public ConnectionSettings(string name, string ipAddress, int port, int bufferSize)
        {
            NameValidatable = new Validatable<string>(name, _nameValidator);
            IpAddressValidatable = new Validatable<string>(ipAddress, _ipValidator);
            PortValidatable = new Validatable<int>(port, _portValidator);
            BufferSizeValidatable = new Validatable<int>(bufferSize, _bufferSizeValidator);
        }

        public ConnectionSettings(string name, int port, int bufferSize)
        {
            NameValidatable = new Validatable<string>(name, _nameValidator);
            IpAddressValidatable = new Validatable<string>(IPAddress.Any.ToString(), _ipValidator);
            PortValidatable = new Validatable<int>(port, _portValidator);
            BufferSizeValidatable = new Validatable<int>(bufferSize, _bufferSizeValidator);
        }
    }
}