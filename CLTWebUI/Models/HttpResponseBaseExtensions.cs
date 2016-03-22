using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace CLTWebUI.Models
{
    public static class HttpResponseBaseExtensions
    {
        public static int SetAuthCookie<T>(this HttpResponseBase responseBase, string name, bool rememberMe, T userData)
        {
            var cookie = FormsAuthentication.GetAuthCookie(name, rememberMe);
            var ticket = FormsAuthentication.Decrypt(cookie.Value);

            var newTicket = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration,
                ticket.IsPersistent, JsonConvert.SerializeObject(userData), ticket.CookiePath);
            var encTicket = FormsAuthentication.Encrypt(newTicket);

            cookie.Value = encTicket;

            responseBase.Cookies.Add(cookie);

            return encTicket.Length;
        }
    }
}