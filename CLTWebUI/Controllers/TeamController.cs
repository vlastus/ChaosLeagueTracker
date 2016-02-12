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
        // GET: Team
        public ActionResult Index()
        {
            using (var ctx = new CLTEntities())
            {
                List<Teams> Teams = ctx.Teams
                    .Where(t => t.Status == Status.Active)
                    .ToList();
                ViewBag.Teams = Teams;
            }
            return View();
        }

        public ActionResult Detail(int teamid)
        {
            TeamDetailViewModel model = new TeamDetailViewModel();
            using (var ctx = new CLTEntities())
            {
                Teams Team = ctx.Teams
                    .Include(t => t.Players.Select(p => p.PlayerTypes))
                    .Include(t => t.Players.Select(p => p.PlayerSkills))
                    .Include(t => t.Users)
                    .Where(t => t.Status == Status.Active && t.ID == teamid)
                    .FirstOrDefault();
                /*
                string cosi = "";
                foreach (Players player in Team.Players)
                {
                    var typeskills = player.PlayerTypes.Skillset.Split('|').ToList();
                    foreach (var ts in typeskills)
                        cosi += Enum.Parse(typeof(Skills),ts);
                }
                */
                model.Team = Team;
            }
            return View(model);
        }
    }
}