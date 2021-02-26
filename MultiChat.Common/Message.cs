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

        public void Append(string content)
        {
            if (content.Contains(Marker)) Terminated = true;
            Builder.Append(content);
        }

        public string Prepare()
        {
            return WebUtility.UrlEncode(Builder.ToString()) + Marker;
        }

        public string Decode()
        {
            Builder.Replace(Marker, string.Empty);
            return Builder.ToString();;
        }
    }
}