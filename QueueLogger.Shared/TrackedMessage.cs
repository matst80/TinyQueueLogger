using Microsoft.Extensions.Logging;
using Microsoft.Azure.ServiceBus;
using System.Text;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace QueueLogger
{
    public interface ISessionObject
    {
        string SessionId { get; set; }
    }
    public class TrackedMessage
    {
        public TrackedMessage(string id, string source, object message) : this(id, message)
        {
            Source = source;
        }

        public TrackedMessage(string id, object message)
        {
            TrackedId = id;

            if (message is string strMessage)
            {
                Message = strMessage;
            }
            else
            {
                try
                {
                    Message = JsonConvert.SerializeObject(message, Formatting.Indented);
                }
                catch
                {
                    Message = message.ToString();
                }
            }
        }

        public string TrackedId { get; set; }
        public string Source { get; set; }
        public string Type { get; set; }
        public bool IsData { get; set; }
        public LogLevel Level { get; set; }
        public string[] Tags { get; set; }
        public string SessionId { get; set; }

        public string Message { get; set; }
        public int EventId { get; set; }

        public Message GetMessage()
        {
            var message = new Message()
            {
                Body = Encoding.UTF8.GetBytes(GetBody())
            };
            PopulatePropertyDictionary(message.UserProperties);
            return message;
        }

        private void PopulatePropertyDictionary(IDictionary<string, object> ret)
        {
            foreach (var prp in this.GetType().GetProperties())
            {
                var val = prp.GetValue(this, null);
                if (val != null)
                {
                    if (prp.PropertyType.IsEnum)
                        val = val.ToString();
                    ret.Add(prp.Name, val);
                }
            }
        }

        public string GetBody()
        {
            var sb = new StringBuilder();
            sb.AppendLine(Message);
            return sb.ToString();
        }
    }
}
