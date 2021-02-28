using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace MultiChat.Common
{
    public class Message
    {
        private const string Marker = @"\r\n";
        private const string SpecialPrefix = @"$";
        private StringBuilder Builder { get; }
        private MessageType Type { get; }
        public bool Terminated { get; private set; }
        public bool Empty => string.IsNullOrEmpty(Builder.ToString());
        public bool Special => Builder.ToString().StartsWith(SpecialPrefix);

        public Message()
        {
            Builder = new StringBuilder();
            Type = MessageType.Regular;
        }

        public Message(string content)
        {
            Builder = new StringBuilder(content);
            Type = MessageType.Regular;
        }
        
        public Message(byte[] bytes)
        {
            var content = Encoding.Unicode.GetString(bytes);
            Builder = new StringBuilder(content);
            Type = MessageType.Regular;
        }

        public Message(string parameter, string value="")
        {
            Builder = new StringBuilder();
            Builder.Append(parameter);
            if (!string.IsNullOrEmpty(value))
            {
                Builder.Append("=");
                Builder.Append(value);
            }
            Type = MessageType.Parameterized;
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
            if (Type == MessageType.Parameterized)
            {
                message = SpecialPrefix + message;
            }
            return Encoding.Unicode.GetBytes(message);
        }

        public string Decode()
        {
            Builder.Replace(Marker, string.Empty);
            Builder.Replace(SpecialPrefix, string.Empty);
            Builder.Replace("\0", string.Empty);
            return WebUtility.UrlDecode(Builder.ToString());
        }

        public string[] GetValues()
        {
            return !Special ? null : Decode().Split("=");
        }
    }
}