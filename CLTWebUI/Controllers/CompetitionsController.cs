using CLT.Data;
using CLTWebUI.Models.Competitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CLTWebUI.Controllers
{
    public class CompetitionsController : Controller
    {
        IUnitOfWork unitOfWork;
        public CompetitionsController(IUnitOfWork uow)
        {
            unitOfWork = uow;
        }

        // GET: Competitions
        public ActionResult Index()
        {
            var model = new CompetitionsViewModel();

            List<Competitions> comps = unitOfWork.CompetitionRepository.Get(includeProperties: "Groups").ToList();
            comps.Reverse();
            model.Competitions = comps;

            return View(model);
        }
    }
}