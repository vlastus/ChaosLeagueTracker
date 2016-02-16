using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CLTWebUI.Models
{
    public class AppMessage
    {
        public string Text { get; set; }
        public MessageSeverity Severity { get; set; }

        public AppMessage (string text, MessageSeverity severity)
        {
            Text = text;
            Severity = severity;
        }
    }

    public enum MessageSeverity : int
    {
        Success = 1,
        Info = 2,
        Warning = 3,
        Danger = 4
    }
}