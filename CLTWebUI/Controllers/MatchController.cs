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
        [Authorize]
        public ActionResult Add(int? fixtureId)
        {
            var fixture = unitOfWork.FixtureRepository.GetByID((int)fixtureId);
            if (fixture == null)
            {
                AddApplicationMessage("Neznámý zápas.", Models.MessageSeverity.Danger);
                return RedirectToAction("Index", "Competitions");
            }
            var players1 = unitOfWork.PlayerRepository.GetPlayersForEvent(fixture.Team1).ToList();
            var players2 = unitOfWork.PlayerRepository.GetPlayersForEvent(fixture.Team2).ToList();
            var i = 0;
            var noplayer = new Players()
            {
                ID = 0,
                Name = "-- Nikdo ze seznamu --"
            };
            foreach (var player in players1)
            {
                players1[i].Name = player.Number + " - " + player.Name;
                i++;
            }
            players1.Add(noplayer);
            i = 0;
            foreach (var player in players2)
            {
                players2[i].Name = player.Number + " - " + player.Name;
                i++;
            }
            players2.Add(noplayer);
            var model = new MatchViewModel()
            {
                fixture = fixture,
                players1 = new SelectList(players1.OrderBy(m => m.ID),"ID","Name"),
                players2 = new SelectList(players2.OrderBy(m => m.ID), "ID", "Name")
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Add(MatchViewModel model)
        {
            var fixture = unitOfWork.FixtureRepository.GetByID(model.fixtureid);
            if (ModelState.IsValid)
            {
                var teamData1 = new TeamMatchData()
                {
                    Team = model.fixture.Team1,
                    Fame = model.fame1,
                    Gate = model.gate1,
                    Winnings = model.winning1,
                    FanFactorMod = model.fanfactor1,
                    MVP = model.mvp1,
                    Score = model.score1
                    //teaminducements
                    //spirallingexpense
                };
                var teamData2 = new TeamMatchData()
                {
                    Team = model.fixture.Team2,
                    Fame = model.fame2,
                    Gate = model.gate2,
                    Winnings = model.winning2,
                    FanFactorMod = model.fanfactor2,
                    MVP = model.mvp2,
                    Score = model.score2
                    //teaminducements
                    //spirallingexpense
                };

                var match = new Matches()
                {
                    TeamMatchData = teamData1,
                    TeamMatchData1 = teamData2,
                    Date = model.matchDate,
                    Competition = fixture.Group,
                    Fixture = fixture.ID,
                    Round = fixture.Round
                };

                if (model.mvp1 > 0)
                {
                    var mvp1 = unitOfWork.PlayerRepository.GetByID(model.mvp1);
                    mvp1.SPP += 5;
                    mvp1.MVP++;
                    unitOfWork.PlayerRepository.Update(mvp1);
                }
                if (model.mvp2 > 0)
                {
                    var mvp2 = unitOfWork.PlayerRepository.GetByID(model.mvp2);
                    mvp2.SPP += 5;
                    mvp2.MVP++;
                    unitOfWork.PlayerRepository.Update(mvp2);
                }

                unitOfWork.TeamMatchDataRepository.Insert(teamData1);
                unitOfWork.TeamMatchDataRepository.Insert(teamData2);
                unitOfWork.MatchRepository.Insert(match);

                unitOfWork.Save();

                foreach(var mevent in model.events)
                {
                    //TODO
                }

                return RedirectToAction("Group", "Team", new { groupid = fixture.Group });
            }
            if (fixture == null)
            {
                AddApplicationMessage("Neznámý zápas.", Models.MessageSeverity.Danger);
                return RedirectToAction("Index", "Competitions");
            }
            var players1 = unitOfWork.PlayerRepository.GetPlayersForEvent(fixture.Team1).ToList();
            var players2 = unitOfWork.PlayerRepository.GetPlayersForEvent(fixture.Team2).ToList();
            var i = 0;
            var noplayer = new Players()
            {
                ID = 0,
                Name = "-- Nikdo ze seznamu --"
            };
            foreach (var player in players1)
            {
                players1[i].Name = player.Number + " - " + player.Name;
                i++;
            }
            players1.Add(noplayer);
            i = 0;
            foreach (var player in players2)
            {
                players2[i].Name = player.Number + " - " + player.Name;
                i++;
            }
            players2.Add(noplayer);
            model.fixture = fixture;
            model.players1 = new SelectList(players1.OrderBy(m => m.ID), "ID", "Name");
            model.players2 = new SelectList(players2.OrderBy(m => m.ID), "ID", "Name");

            return View(model);
        }

        [Authorize]
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
                case MatchEventsTypes.Interception:
                    template = "Interception";
                    break;
                case MatchEventsTypes.Completion:
                    template = "Completion";
                    break;
                case MatchEventsTypes.Injury:
                    template = "Injury";
                    break;
            }

            model.events = new List<MatchEventModel.EventDetailModel>();
            model.events.Add(mevent);

            return View(template,model);
        }
    }
}