using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using CLT.Data;
using CLTWebUI.Models.Team;
using CLTWebUI.Models;

namespace CLTWebUI.Controllers
{
    public class TeamController : BaseController
    {
        IUnitOfWork unitOfWork;

        public static Dictionary<Races, int> RerollPrices = new Dictionary<Races, int>
        {
            {Races.Amazon, 50000},
            {Races.Chaos, 60000},
            {Races.ChaosDwarf, 70000},
            {Races.DarkElf, 50000},
            {Races.Dwarf, 50000},
            {Races.Elf, 50000},
            {Races.Goblin, 60000},
            {Races.Halfling, 60000},
            {Races.HighElf, 50000},
            {Races.Human, 50000},
            {Races.Khemri, 70000},
            {Races.Lizardman, 60000},
            {Races.Necromantic, 70000},
            {Races.Norse, 60000},
            {Races.Nurgle, 70000},
            {Races.Ogre, 70000},
            {Races.Orc, 60000},
            {Races.Skaven, 60000},
            {Races.Undead, 70000},
            {Races.Vampire, 70000},
            {Races.WoodElf, 50000},
            {Races.ChaosPact, 70000},
            {Races.Slann, 50000},
            {Races.Underworld, 70000}
        };

        public TeamController(IUnitOfWork uow)
        {
            unitOfWork = uow;
        }
        // GET: Team
        [RoleAuthorize(Roles = "Admin,SuperAdmin")]
        public ActionResult Index()
        {
            var model = new TeamListViewModel()
            {
                teams = unitOfWork.TeamRepository.GetTeams().ToList()
            };
            return View(model);
        }

        public ActionResult Group(int groupid)
        {
            TeamListViewModel model = new TeamListViewModel()
            {
                group = unitOfWork.GroupRepository.GetByID(groupid),
                //teams = unitOfWork.TeamRepository.GetTeamsByGroup(groupid).ToList(),
                table = unitOfWork.GroupTableRepository.Get(filter: gt => gt.ID==groupid).OrderBy(gt=>gt.Poradi).ToList(),
                fixtures = unitOfWork.FixtureRepository.Get(filter: f => f.Group == groupid).GroupBy(f => f.Round)
            };
            return View(model);
        }

        public ActionResult Detail(int? teamid)
        {
            if (teamid == null)
            {
                ViewBag.Message = "Id týmu nebylo uvedeno.";
                return RedirectToAction("Error","Home");
            }

            TeamDetailViewModel model = new TeamDetailViewModel();
            model.Team = unitOfWork.TeamRepository
                .Get(
                    filter: t => (t.ID == teamid && t.Status == Status.Active), 
                    includeProperties: "Users,Players")
                .FirstOrDefault();
            model.TeamId = model.Team.ID;

            var playerTypes = unitOfWork.PlayerTypeRepository.Get(filter: pt => pt.Race == model.Team.Race).ToList();
            model.playertypes = new SelectList(playerTypes, "ID", "Name");

            return View(model);
        }

        [HttpGet]
        [RoleAuthorize(Roles = "Admin, SuperAdmin")]
        public ActionResult Add()
        {
            var model = new AddTeamViewModel();
            var races = from Races r in Enum.GetValues(typeof(Races)) select new { ID = (int)r, Name = r.ToString() };
            model.races = new SelectList(races, "ID", "Name");
            var users = unitOfWork.UserRepository.Get(filter: u => u.Status == Status.Active).ToList();
            model.users = new SelectList(users, "ID", "Name");
            return View(model);
        }

        public ActionResult DetailForPrint(int? teamid)
        {
            if (teamid == null)
            {
                ViewBag.Message = "Id týmu nebylo uvedeno.";
                return View();
            }

            TeamDetailViewModel model = new TeamDetailViewModel();
            model.Team = unitOfWork.TeamRepository
                .Get(
                    filter: t => (t.ID == teamid && t.Status == Status.Active),
                    includeProperties: "Users,Players")
                .FirstOrDefault();
            model.TeamId = model.Team.ID;

            return View(model);
        }

        [HttpPost]
        [RoleAuthorize(Roles = "Admin, SuperAdmin")]
        public ActionResult Add(AddTeamViewModel model)
        {
            var races = from Races r in Enum.GetValues(typeof(Races)) select new { ID = (int)r, Name = r.ToString() };
            model.races = new SelectList(races, "ID", "Name");
            var users = unitOfWork.UserRepository.Get(filter: u => u.Status == Status.Active).ToList();
            model.users = new SelectList(users, "ID", "Name");
            if (ModelState.IsValid)
            {
                Teams team = new Teams()
                {
                    Race = (Races)model.race,
                    Owner = model.owner,
                    Name = model.name,
                    Rerolls = 0,
                    Fanfactor = 0,
                    Asscoaches = 0,
                    Cheerleaders = 0,
                    Apothecary = 0,
                    Value = 0,
                    Treasury = model.startValue == null ? model.startValue : 1000000,
                    Status = Status.Active
                };
                unitOfWork.TeamRepository.Insert(team);
                unitOfWork.Save();
                AddApplicationMessage("Tým byl úspěšně založen", MessageSeverity.Success);
                return RedirectToAction("Index", "Team");
            }
            AddApplicationMessage("Tým se nepodařilo založit, zkontrolujte formulář", MessageSeverity.Danger);
            return View(model);
        }

        [RoleAuthorize(Roles = "Admin, SuperAdmin")]
        public ActionResult Delete(int? teamid)
        {
            if (teamid == null)
                AddApplicationMessage("Je nutné zadat ID týmu pro deaktivaci", MessageSeverity.Danger);
            else
            {
                var team = unitOfWork.TeamRepository.GetByID(teamid);
                if (team == null)
                    AddApplicationMessage("Tým nenalezen", MessageSeverity.Danger);
                else
                {
                    team.Status = Status.Inactive;
                    unitOfWork.TeamRepository.Update(team);
                    unitOfWork.Save();
                    AddApplicationMessage("Tým byl deaktivován", MessageSeverity.Success);
                }
            }
            return RedirectToAction("Index", "Team");
        }

        [HttpGet]
        [RoleAuthorize(Roles = "Admin, SuperAdmin")]
        public ActionResult Edit(int? teamid)
        {
            if (teamid == null)
            {
                AddApplicationMessage("Je třeba zadat ID týmu", MessageSeverity.Danger);
                return RedirectToAction("Index", "Team");
            }
            var team = unitOfWork.TeamRepository.GetByID(teamid);
            if (team == null)
            {
                AddApplicationMessage("Tým nebyl nalezen", MessageSeverity.Danger);
                return RedirectToAction("Index", "Team");
            }

            var races = from Races r in Enum.GetValues(typeof(Races)) select new { ID = (int)r, Name = r.ToString() };
            var users = unitOfWork.UserRepository.Get(filter: u => u.Status == Status.Active).ToList();

            var model = new AddTeamViewModel()
            {
               team = team,
               owner = team.Owner,
               race = (int)team.Race,
               races = new SelectList(races, "ID", "Name"),
               users = new SelectList(users, "ID", "Name")
            };
            return View(model);
        }

        [HttpPost]
        [RoleAuthorize(Roles = "Admin, SuperAdmin")]
        public ActionResult Edit(AddTeamViewModel model)
        {
            var team = unitOfWork.TeamRepository.GetByID(model.teamid);
            var races = from Races r in Enum.GetValues(typeof(Races)) select new { ID = (int)r, Name = r.ToString() };
            var users = unitOfWork.UserRepository.Get(filter: u => u.Status == Status.Active).ToList();

            if (!ModelState.IsValid)
            {
                model.team = team;
                model.races = new SelectList(races, "ID", "Name");
                model.users = new SelectList(users, "ID", "Name");
                AddApplicationMessage("Tým se nepodařilo uložit, zkontrolujte formulář", MessageSeverity.Danger);
                return View(model);
            }

            team.Name = model.name;
            team.Owner = model.owner;
            //team.Race = model.race;
            team.Rerolls = model.rerolls;
            team.Fanfactor = model.fanfactor;
            team.Asscoaches = model.asscoaches;
            team.Cheerleaders = model.cheerleaders;
            team.Apothecary = model.apothecary == 1 ? model.apothecary : 0;
            team.Value = model.value;
            team.Treasury = model.treasury;

            unitOfWork.TeamRepository.Update(team);
            unitOfWork.Save();
            AddApplicationMessage("Tým byl uložen", MessageSeverity.Success);

            return RedirectToAction("Index", "Team");
        }

        [Authorize]
        public ActionResult Buy(int? teamid, string what)
        {
            var team = unitOfWork.TeamRepository.GetByID(teamid);
            if (team == null)
                AddApplicationMessage("Neznámé ID týmu", MessageSeverity.Danger);
            else
            {
                switch (what)
                {
                    case "reroll":
                        if (team.Treasury < RerollPrices[team.Race])
                        {
                            AddApplicationMessage("Na reroll nemá tým dost peněz", MessageSeverity.Danger);
                        }
                        else if (team.Rerolls > 7)
                        {
                            AddApplicationMessage("Více rerollů už není možno koupit", MessageSeverity.Danger);
                        }
                        else
                        {
                            team.Rerolls++;
                            team.Treasury -= RerollPrices[team.Race];
                            team.Value += RerollPrices[team.Race];
                            unitOfWork.TeamRepository.Update(team);
                            unitOfWork.Save();
                            AddApplicationMessage("Nákup rerollu byl úspěšný", MessageSeverity.Success);
                        }
                        break;
                    case "cheer":
                        if (team.Treasury < 10000)
                        {
                            AddApplicationMessage("Na roztleskávačky nemá tým dost peněz", MessageSeverity.Danger);
                        }
                        else
                        {
                            team.Cheerleaders++;
                            team.Treasury -= 10000;
                            team.Value += 10000;
                            unitOfWork.TeamRepository.Update(team);
                            unitOfWork.Save();
                            AddApplicationMessage("Nákup roztleskávaček byl úspěšný", MessageSeverity.Success);
                        }
                        break;
                    case "assist":
                        if (team.Treasury < 10000)
                        {
                            AddApplicationMessage("Na asistenty trenéra nemá tým dost peněz", MessageSeverity.Danger);
                        }
                        else
                        {
                            team.Asscoaches++;
                            team.Treasury -= 10000;
                            team.Value += 10000;
                            unitOfWork.TeamRepository.Update(team);
                            unitOfWork.Save();
                            AddApplicationMessage("Nákup asistenta trenéra byl úspěšný", MessageSeverity.Success);
                        }
                        break;
                    case "apothecary":
                        if (team.Treasury < 50000)
                        {
                            AddApplicationMessage("Na felčara nemá tým dost peněz", MessageSeverity.Danger);
                        }
                        else if (team.Apothecary > 0)
                        {
                            AddApplicationMessage("Více felčarů už není možno koupit", MessageSeverity.Danger);
                        }
                        else if (team.Race == Races.Undead || team.Race == Races.Necromantic || team.Race == Races.Khemri)
                        {
                            AddApplicationMessage("Tento tým felčara mít nemůže", MessageSeverity.Danger);
                        }
                        else
                        {
                            team.Apothecary = 1;
                            team.Treasury -= 50000;
                            team.Value += 50000;
                            unitOfWork.TeamRepository.Update(team);
                            unitOfWork.Save();
                            AddApplicationMessage("Nákup felčara byl úspěšný", MessageSeverity.Success);
                        }
                        break;
                }
                return RedirectToAction("Detail", "Team", new { teamid = team.ID });
            }
            return RedirectToAction("Index","Competitions");
        }
    }
}