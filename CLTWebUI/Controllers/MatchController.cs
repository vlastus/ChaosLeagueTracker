using CLT.Data;
using CLTWebUI.Models.Match;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CLTWebUI.Controllers
{
    public class MatchController : BaseController
    {
        IUnitOfWork unitOfWork;

        public MatchController(IUnitOfWork uow)
        {
            unitOfWork = uow;
        }

        [HttpGet]
        public ActionResult Add(int? fixtureId)
        {
            var fixture = unitOfWork.FixtureRepository.GetByID((int)fixtureId);
            if (fixture == null)
            {
                AddApplicationMessage("Neznámý zápas.", Models.MessageSeverity.Danger);
                return RedirectToAction("Index", "Competitions");
            }
            var model = new MatchViewModel()
            {
                fixture = fixture
            };
            return View(model);
        }
    }
}