using CLT.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CLTWebUI.Controllers
{
    public class MatchController : Controller
    {
        IUnitOfWork unitOfWork;

        public MatchController(IUnitOfWork uow)
        {
            unitOfWork = uow;
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
    }
}