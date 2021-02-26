using System.Diagnostics;
using System.Net;
using System.Text;

namespace MultiChat.Common
{
    public class Message
    {
        private const string Marker = @"\r\n";
        private StringBuilder Builder { get; }
        public bool Terminated { get; private set; }
        public bool Empty => string.IsNullOrEmpty(Builder.ToString());

        public Message()
        {
            Builder = new StringBuilder();
        }

        public Message(string content)
        {
            Builder = new StringBuilder(content);
        }
        
        public Message(byte[] bytes)
        {
            var content = Encoding.Unicode.GetString(bytes);
            Builder = new StringBuilder(content);
        }

        public void Append(byte[] bytes)
        {
            Append(Encoding.Unicode.GetString(bytes));
        }
        
        public void Append(string content)
        {
            if (content.Contains(Marker)) Terminated = true;
            Builder.Append(content);
        }

        public byte[] Prepare()
        {
            var message = WebUtility.UrlEncode(Builder.ToString()) + Marker;
            return Encoding.Unicode.GetBytes(message);
        }

        public string Decode()
        {
            Builder.Replace(Marker, string.Empty);
            return WebUtility.UrlDecode(Builder.ToString());
        }
    }
}