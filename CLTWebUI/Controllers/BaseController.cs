using CLTWebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;

namespace CLTWebUI.Controllers
{
    public abstract class BaseController : Controller
    {
        protected static Logger dblog = LogManager.GetLogger("dblogger");
        protected static Logger filelog = LogManager.GetLogger("filelogger");

        public void AddApplicationMessage(string message, MessageSeverity severity)
        {
            List<AppMessage> messages = TempData["AppMessages"] as List<AppMessage> ?? new List<AppMessage>();
            messages.Add(new AppMessage(message, severity));
            TempData["AppMessages"] = messages;
        }

        protected void Log(string message, string action, int author, int entityid, string entitytype)
        {
            LogEventInfo levent = new LogEventInfo(LogLevel.Info, "", message);
            levent.Properties["action"] = action;
            levent.Properties["author"] = author;
            levent.Properties["entityid"] = entityid;
            levent.Properties["entitytype"] = entitytype;
            dblog.Log(levent);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            filterContext.ExceptionHandled = true;

            filelog.Error(filterContext.Exception, "An exception occured");

            filterContext.Result = RedirectToAction("Index", "Error");
        }
    }
}