using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using CLT.Data;
using CLTWebUI.Models.Team;

namespace CLTWebUI.Controllers
{
    public class TeamController : Controller
    {
        IUnitOfWork unitOfWork;

        public TeamController(IUnitOfWork uow)
        {
            unitOfWork = uow;
        }
        // GET: Team
        public ActionResult Index()
        {
            ViewBag.Teams = unitOfWork.TeamRepository.GetTeams();
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
            var playerTypes = unitOfWork.PlayerTypeRepository.Get(filter: pt => pt.Race == model.Team.Race).ToList();
            model.playertypes = new SelectList(playerTypes, "ID", "Name");

            /*
            using (var ctx = new CLTEntities())
            {
                Teams Team = ctx.Teams
                    .Include(t => t.Players.Select(p => p.PlayerTypes))
                    .Include(t => t.Players.Select(p => p.PlayerSkills))
                    .Include(t => t.Users)
                    .Where(t => t.Status == Status.Active && t.ID == teamid)
                    .FirstOrDefault();
                string cosi = "";
                foreach (Players player in Team.Players)
                {
                    var typeskills = player.PlayerTypes.Skillset.Split('|').ToList();
                    foreach (var ts in typeskills)
                        cosi += Enum.Parse(typeof(Skills),ts);
                }
                model.Team = Team;
            }
            */
            return View(model);
        }
    }
}