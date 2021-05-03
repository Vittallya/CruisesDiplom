using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Components
{
    public class NotificationItem
    {
        public string Header { get; }
        public string Body { get; }
        public NotificationType Type { get; }
        
        public NotificationItem(string body, string header = null, NotificationType type = NotificationType.New)
        {
            Header = header;
            Body = body;
            Type = type;
        }
    }
    public enum NotificationType
    {
        New, Success, Fail
    }
}
