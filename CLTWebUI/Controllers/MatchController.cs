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

        public ActionResult Add(MatchViewModel model)
        {
            var x = 1;
            return View();
        }

        public ActionResult AddMatchEventForm(MatchEventsTypes etype, int? teamid, int? fixtureid, int? order)
        {
            var fixture = unitOfWork.FixtureRepository.GetByID(fixtureid);
            var players = unitOfWork.PlayerRepository.GetPlayersForEvent((int)teamid);
            var model = new MatchEventModel()
            {
                players = players.ToList(),
                fixture = fixture,
                eventNo = (int)order
            };
            var targetteam = (teamid == fixture.Team1) ? fixture.Team2 : fixture.Team1;


            var mevent = new MatchEventModel.EventDetailModel()
            {
                eventNo = (int)order,
                type = etype,
                sourcePlayerTeam = (int)teamid
            };

            string template = "";
            switch (etype)
            {
                case MatchEventsTypes.Touchdown:
                    template = "Touchdown";
                    break;
                case MatchEventsTypes.Casualty:
                    template = "Casualty";
                    var players2 = unitOfWork.PlayerRepository.GetPlayersForEvent((int)targetteam);
                    model.players2 = players2.ToList();
                    mevent.targetPlayerTeam = (int)targetteam;
                    break;
                case MatchEventsTypes.Completion:
                    template = "Interception";
                    break;
            }

            model.events = new List<MatchEventModel.EventDetailModel>();
            model.events.Add(mevent);

            return View(template,model);
        }
    }
}