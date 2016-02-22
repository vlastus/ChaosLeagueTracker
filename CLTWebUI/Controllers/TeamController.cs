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
        [RoleAuthorize(Roles="Admin,SuperAdmin")]
        public ActionResult Index()
        {
            ViewBag.Teams = unitOfWork.TeamRepository.GetTeams();
            ViewBag.Competitions = unitOfWork.CompetitionRepository.Get(filter: c => c.Status == Status.Active).ToDictionary(c => c.ID, c => c.Name);
            ViewBag.Groups = unitOfWork.GroupRepository.Get().ToDictionary(c => c.ID, c => c.Name);
            return View();
        }

        public ActionResult Group(int groupid)
        {
            TeamListViewModel model = new TeamListViewModel();

            model.group = unitOfWork.GroupRepository.GetByID(groupid);
            model.teams = unitOfWork.TeamRepository.GetTeamsByGroup(groupid).ToList();

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

        [RoleAuthorize(Roles = "Admin, SuperAdmin")]
        public ActionResult Add(AddTeamViewModel model)
        {
            var races = from Races r in Enum.GetValues(typeof(Races)) select new { ID = (int)r, Name = r.ToString()};
            model.races = new SelectList(races, "ID","Name");
            var users = unitOfWork.UserRepository.Get(filter: u => u.Status == Status.Active).ToList();
            model.users = new SelectList(users, "ID", "Name");
            return View(model);
        }
    }
}