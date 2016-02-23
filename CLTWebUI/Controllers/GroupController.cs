using CLT.Data;
using CLTWebUI.Models;
using CLTWebUI.Models.Team;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CLTWebUI.Controllers
{
    public class GroupController : BaseController
    {
        IUnitOfWork unitOfWork;
        // GET: Group 
        public GroupController(IUnitOfWork uow)
        {
            unitOfWork = uow;
        }

        [RoleAuthorize(Roles = "Admin, SuperAdmin")]
        public ActionResult Admin(int groupid)
        {
            //var teams = unitOfWork.TeamRepository.GetTeamsByGroup(groupid).ToList();
            var allteams = unitOfWork.TeamRepository.Get(filter: t => t.Status == Status.Active).ToList();
            var teams = allteams.FindAll(t => t.CompTeams.Any(ct => ct.Competititon == groupid));
            var availableTeams = allteams.FindAll(t => t.CompTeams.All(ct => ct.Groups.Competitions.Status == Status.Inactive) || t.CompTeams == null && t.CompTeams.All(ct => ct.Competititon != groupid));

            TeamListViewModel model = new TeamListViewModel()
            {
                group = unitOfWork.GroupRepository.GetByID(groupid),
                teams = teams,
                teams2join = availableTeams
            };
            return View(model);
        }

        [RoleAuthorize(Roles = "Admin, SuperAdmin")]
        public ActionResult AssignTeam(int? groupid, int? teamid)
        {
            if (groupid == null || teamid == null)
            {
                AddApplicationMessage("Špatné parametry pro přiřazení týmu do skupiny", MessageSeverity.Danger);
            }
            else
            {
                var team = unitOfWork.TeamRepository.GetByID(teamid);
                var group = unitOfWork.GroupRepository.GetByID(groupid);
                if (team == null || group == null)
                {
                    AddApplicationMessage("Tým nebo skupinu se nepodařilo nalézt", MessageSeverity.Danger);
                }
                else
                {
                    var ct = new CompTeams()
                    {
                        Competititon = (int)groupid,
                        Team = (int)teamid
                    };

                    unitOfWork.CompTeamRepository.Insert(ct);
                    unitOfWork.Save();
                    AddApplicationMessage("Tým byl přiřazen ke skupině", MessageSeverity.Success);
                }
            }

            return RedirectToAction("Admin", "Group", new { groupid = groupid });
        }

        [RoleAuthorize(Roles = "Admin, SuperAdmin")]
        public ActionResult DeassignTeam(int? groupid, int? teamid)
        {
            if (groupid == null || teamid == null)
            {
                AddApplicationMessage("Špatné parametry pro vyřazení týmu ze skupiny", MessageSeverity.Danger);
            }
            else
            {
                var ct = unitOfWork.CompTeamRepository.Get(filter: c => c.Team == teamid && c.Competititon == groupid).FirstOrDefault();
                if (ct == null)
                {
                    AddApplicationMessage("Tým nebo skupinu se nepodařilo nalézt", MessageSeverity.Danger);
                }
                else
                {
                    unitOfWork.CompTeamRepository.Delete(ct);
                    unitOfWork.Save();
                    AddApplicationMessage("Tým byl vyřazen ze skupiny", MessageSeverity.Success);
                }
            }

            return RedirectToAction("Admin", "Group", new { groupid = groupid });
        }
    }
}