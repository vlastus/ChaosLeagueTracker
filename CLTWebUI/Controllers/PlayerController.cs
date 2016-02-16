using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CLT.Data;

namespace CLTWebUI.Controllers
{
    public class PlayerController : Controller
    {
        // GET: Player
        public ActionResult Add()
        {
            return View();
        }
    }
}