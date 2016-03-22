using CLT.Data;
using CLTWebUI.Models;
using CLTWebUI.Models.Account;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CLTWebUI.Controllers
{
    public class AccountController : BaseController
    {
        IUnitOfWork unitOfWork;

        public AccountController(UnitOfWork uow)
        {
            unitOfWork = uow;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                Users user = unitOfWork.UserRepository.Get(filter: u => u.Name == model.name && u.Authentication == model.password).FirstOrDefault();

                if (user != null)
                {

                    UserData userData = new UserData
                    {
                        Name = user.Name,
                        ID = user.ID
                    };
                    Response.SetAuthCookie(user.Name, false, userData);

                    Log("", LogEvent.Login.ToString(), user.ID, "user");

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Chybné uživatelské jméno nebo heslo");
                    AddApplicationMessage("Nepodařilo se přihlásit uživatele", Models.MessageSeverity.Danger);
                }
            }

            return View(model);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }
    }
}