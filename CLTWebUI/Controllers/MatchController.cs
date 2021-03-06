﻿using CLT.Data;
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

        public ActionResult Detail(int? matchid)
        {
            var match = unitOfWork.MatchRepository.GetByID(matchid);
            var fixture = unitOfWork.FixtureRepository.GetByID(match.Fixture);
            var starplayers = unitOfWork.PlayerRepository.Get(filter: p => p.Teams.Race == Races.Special);
            var model = new MatchViewModel()
            {
                match = match,
                fixture = fixture,
                starplayers = new SelectList(starplayers, "ID", "Name")
            };
            return View(model);
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
            var starplayers = unitOfWork.PlayerRepository.Get(filter: p => p.PlayerTypes.Race == Races.Special).ToList();
            starplayers.Concat(unitOfWork.PlayerRepository.GetSpecialsForEvent());
            var inducements = from Inducements r in Enum.GetValues(typeof(Inducements)) select new { ID = (int)r, Name = r.ToString() };

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
                players2 = new SelectList(players2.OrderBy(m => m.ID), "ID", "Name"),
                inducements = new SelectList(inducements, "ID", "Name"),
                starplayers = new SelectList(starplayers, "ID", "Name")
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Add(MatchViewModel model)
        {
            var fixture = unitOfWork.FixtureRepository.GetByID(model.fixtureid);
            Players player1 = null;
            Players player2 = null;

            if (ModelState.IsValid)
            {
                Teams team1 = unitOfWork.TeamRepository.GetByID(fixture.Team1);
                Teams team2 = unitOfWork.TeamRepository.GetByID(fixture.Team2);

                //odstraneni MNG flagu
                RemovePlayerInjuries(team1);
                RemovePlayerInjuries(team2);

                // nove detaily zapasu
                var teamData1 = new TeamMatchData()
                {
                    Team = fixture.Team1,
                    Fame = model.fame1,
                    Gate = model.gate1,
                    Winnings = model.winning1,
                    FanFactorMod = model.fanfactor1,
                    Score = model.score1,
                    SpirallingExpense = CalculateRollingExpenses(fixture.Teams)
                    //teaminducements
                };
                if (model.mvp1 != 0)
                {
                    teamData1.MVP = model.mvp1;
                }

                var teamData2 = new TeamMatchData()
                {
                    Team = fixture.Team2,
                    Fame = model.fame2,
                    Gate = model.gate2,
                    Winnings = model.winning2,
                    FanFactorMod = model.fanfactor2,
                    Score = model.score2,
                    SpirallingExpense = CalculateRollingExpenses(fixture.Teams1)
                    //teaminducements
                };
                if (model.mvp2 != 0)
                {
                    teamData2.MVP = model.mvp2;
                }

                // pricteni penez, update ff, rolling expense
                team1.Treasury += model.winning1;
                team2.Treasury += model.winning2;
                if (CalculateRollingExpenses(team1)>0)
                    team1.Treasury = (team1.Treasury- CalculateRollingExpenses(team1) >0) ? team1.Treasury -= CalculateRollingExpenses(team1) : 0;
                if (CalculateRollingExpenses(team2) > 0)
                    team2.Treasury = (team2.Treasury - CalculateRollingExpenses(team2) > 0) ? team2.Treasury -= CalculateRollingExpenses(team2) : 0;
                team1.Fanfactor += model.fanfactor1;
                team1.Value += model.fanfactor1 * 10000;
                team2.Fanfactor += model.fanfactor2;
                team2.Value += model.fanfactor2 * 10000;

                // vlozeni zapasu
                var match = new Matches()
                {
                    TeamMatchData = teamData1,
                    TeamMatchData1 = teamData2,
                    Date = model.matchDate,
                    Competition = fixture.Group,
                    Fixture = fixture.ID,
                    Round = fixture.Round
                };

                // pridani MVP 
                if (model.mvp1 > 0)
                {
                    player1 = unitOfWork.PlayerRepository.GetByID(model.mvp1);
                    player1.SPP += 5;
                    player1.MVP++;
                    unitOfWork.PlayerRepository.Update(player1);
                }
                if (model.mvp2 > 0)
                {
                    player2 = unitOfWork.PlayerRepository.GetByID(model.mvp2);
                    player2.SPP += 5;
                    player2.MVP++;
                    unitOfWork.PlayerRepository.Update(player2);
                }

                //Inducementy
                if (model.selectedInducements1 != null) foreach(var ind in model.selectedInducements1)
                {
                    var inducement = new TeamInducements()
                    {
                        Type = ind,
                        Value = 0
                    };
                    teamData1.TeamInducements.Add(inducement);
                }
                if (model.selectedStarplayers1 != null) foreach (var ind in model.selectedStarplayers1)
                {
                    var inducement = new TeamInducements()
                    {
                        Type = Inducements.StarPlayer,
                        Value = ind
                    };
                    teamData1.TeamInducements.Add(inducement);
                }
                if (model.selectedInducements2 != null) foreach (var ind in model.selectedInducements2)
                {
                    var inducement = new TeamInducements()
                    {
                        Type = ind,
                        Value = 0
                    };
                    teamData2.TeamInducements.Add(inducement);
                }
                if (model.selectedStarplayers2 != null) foreach (var ind in model.selectedStarplayers2)
                {
                    var inducement = new TeamInducements()
                    {
                        Type = Inducements.StarPlayer,
                        Value = ind
                    };
                    teamData2.TeamInducements.Add(inducement);
                }

                unitOfWork.TeamMatchDataRepository.Insert(teamData1);
                unitOfWork.TeamMatchDataRepository.Insert(teamData2);
                unitOfWork.TeamRepository.Update(team1);
                unitOfWork.TeamRepository.Update(team2);
                unitOfWork.MatchRepository.Insert(match);

                unitOfWork.Save();

                // vyreseni match eventu
                if (model.events != null) foreach (var mevent in model.events)
                {
                    if (mevent.canceled)
                        continue;

                    player1 = unitOfWork.PlayerRepository.GetByID(mevent.sourcePlayer);
                    MatchEvents newEvent = new MatchEvents()
                    {
                        Team = mevent.sourcePlayerTeam,
                        SourcePlayer = mevent.sourcePlayer,
                        Match = match.ID,
                        EventType = (int)mevent.type
                    };
                    switch (mevent.type)
                    {
                        case MatchEventsTypes.Touchdown:
                            player1.SPP += 3;
                            player1.TD++;
                            break;
                        case MatchEventsTypes.Completion:
                            player1.SPP++;
                            player1.COMP++;
                            break;
                        case MatchEventsTypes.Interception:
                            player1.SPP += 2;
                            player1.INT++;
                            break;
                        case MatchEventsTypes.Injury:
                            var tteam1 = unitOfWork.TeamRepository.GetByID(player1.Team);
                            switch (mevent.result)
                            {
                                case 1:
                                    break;
                                case 2:
                                    player1.MNG = 1;
                                    tteam1.Value -= player1.Value;
                                    break;
                                case 3:
                                    player1.NI = 1;
                                    player1.MNG = 1;
                                    tteam1.Value -= player1.Value;
                                    break;
                                case 4:
                                    player1.MA--;
                                    player1.MNG = 1;
                                    tteam1.Value -= player1.Value;
                                    break;
                                case 5:
                                    player1.AV--;
                                    player1.MNG = 1;
                                    tteam1.Value -= player1.Value;
                                    break;
                                case 6:
                                    player1.AG--;
                                    player1.MNG = 1;
                                    tteam1.Value -= player1.Value;
                                    break;
                                case 7:
                                    player1.ST--;
                                    player1.MNG = 1;
                                    tteam1.Value -= player1.Value;
                                    break;
                                case 8:
                                    player1.Status = Status.Inactive;
                                    tteam1.Value -= player1.Value;
                                    break;
                            }
                            if (fixture.Groups.Playoff == 1 && player1.MNG == 1 && player1.Status == Status.Active)
                            {
                                    player1.MNG = 0;
                                    tteam1.Value += player1.Value;
                            }
                                unitOfWork.TeamRepository.Update(tteam1);
                            break;
                        case MatchEventsTypes.Casualty:
                            player1.SPP += 2;
                            player1.CAS++;
                            player2 = unitOfWork.PlayerRepository.GetByID(mevent.targetPlayer);
                            newEvent.TargetPlayer = mevent.targetPlayer;
                            newEvent.Result = mevent.result;
                            var tteam2 = unitOfWork.TeamRepository.GetByID(player2.Team);
                            switch (mevent.result)
                            {
                                case 1:
                                    break;
                                case 2:
                                    player2.MNG = 1;
                                    tteam2.Value -= player2.Value;
                                    break;
                                case 3:
                                    player2.NI = 1;
                                    player2.MNG = 1;
                                    tteam2.Value -= player2.Value;
                                    break;
                                case 4:
                                    player2.MA--;
                                    player2.MNG = 1;
                                    tteam2.Value -= player2.Value;
                                    break;
                                case 5:
                                    player2.AV--;
                                    player2.MNG = 1;
                                    tteam2.Value -= player2.Value;
                                    break;
                                case 6:
                                    player2.AG--;
                                    player2.MNG = 1;
                                    tteam2.Value -= player2.Value;
                                    break;
                                case 7:
                                    player2.ST--;
                                    player2.MNG = 1;
                                    tteam2.Value -= player2.Value;
                                    break;
                                case 8:
                                    player1.Kills++;
                                    player2.Status = Status.Inactive;
                                    tteam2.Value -= player2.Value;
                                    break;
                            }
                            if (fixture.Groups.Playoff == 1 && player2.MNG == 1 && player2.Status == Status.Active)
                            {
                                player2.MNG = 0;
                                tteam2.Value += player2.Value;
                            }
                            unitOfWork.PlayerRepository.Update(player2);
                            unitOfWork.TeamRepository.Update(tteam2);
                            break;
                    }
                    unitOfWork.MatchEventRepository.Insert(newEvent);
                    unitOfWork.PlayerRepository.Update(player1);
                    unitOfWork.Save();
                }

                AddApplicationMessage("Zápas byl uložen", Models.MessageSeverity.Success);
                return RedirectToAction("Group", "Team", new { groupid = fixture.Group });
            }
            if (fixture == null)
            {
                AddApplicationMessage("Neznámý zápas.", Models.MessageSeverity.Danger);
                return RedirectToAction("Index", "Competitions");
            }
            AddApplicationMessage("Formulář obsahuje chyby, zkontrolujte prosím obsah",Models.MessageSeverity.Danger);
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
            players1.AddRange(unitOfWork.PlayerRepository.GetSpecialsForEvent());
            players2.AddRange(unitOfWork.PlayerRepository.GetSpecialsForEvent());
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
            var starplayers = unitOfWork.PlayerRepository.Get(filter: p => p.PlayerTypes.Race == Races.Special).ToList();
            starplayers.Concat(unitOfWork.PlayerRepository.GetSpecialsForEvent());
            var inducements = from Inducements r in Enum.GetValues(typeof(Inducements)) select new { ID = (int)r, Name = r.ToString() };
            model.inducements = new SelectList(inducements, "ID", "Name");
            model.starplayers = new SelectList(starplayers, "ID", "Name");

            return View(model);
        }

        [Authorize]
        public ActionResult AddMatchEventForm(MatchEventsTypes etype, int? teamid, int? fixtureid, int? order)
        {
            var fixture = unitOfWork.FixtureRepository.GetByID(fixtureid);
            var players = unitOfWork.PlayerRepository.GetPlayersForEvent((int)teamid).ToList();
            players.AddRange(unitOfWork.PlayerRepository.GetSpecialsForEvent());
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
                    var players2 = unitOfWork.PlayerRepository.GetPlayersForEvent((int)targetteam).ToList();
                    players2.AddRange(unitOfWork.PlayerRepository.GetSpecialsForEvent());
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

        public int CalculateRollingExpenses(Teams team)
        {
            return (team.Value - 1750000 < 0) ? 0 : ((int)team.Value - 1750000) / 150000 * 10000;
        }

        public void RemovePlayerInjuries(Teams team)
        {
            var players = unitOfWork.PlayerRepository.Get(filter: p => p.Team == team.ID && p.MNG == 1);
            foreach(var player in players)
            {
                if (player.MNG == 1)
                {
                    player.MNG = 0;
                    team.Value += player.Value;
                    unitOfWork.PlayerRepository.Update(player);
                }
            }
            unitOfWork.TeamRepository.Update(team);
            unitOfWork.Save();
        }
    }
}