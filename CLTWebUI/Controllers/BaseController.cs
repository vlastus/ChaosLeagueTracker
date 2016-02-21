using CLTWebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CLTWebUI.Controllers
{
    public abstract class BaseController : Controller
    {
        public void AddApplicationMessage(string message, MessageSeverity severity)
        {
            List<AppMessage> messages = TempData["AppMessages"] as List<AppMessage> ?? new List<AppMessage>();
            messages.Add(new AppMessage(message, severity));
            TempData["AppMessages"] = messages;
        }
    }
}