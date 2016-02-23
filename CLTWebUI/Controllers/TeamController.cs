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
                teams = unitOfWork.TeamRepository.GetTeamsByGroup(groupid).ToList()
            };
            return View(model);
        }

        public ActionResult Detail(int? teamid)
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
    }
}