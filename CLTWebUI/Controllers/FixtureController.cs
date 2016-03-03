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
    public class FixtureController : BaseController
    {
        IUnitOfWork unitOfWork;

        public FixtureController(IUnitOfWork uow)
        {
            unitOfWork = uow;
        }
        
        // GET: Fixture
        public ActionResult Index(int? groupid)
        {
            Groups group = unitOfWork.GroupRepository.GetByID((int)groupid);
            if  (group == null)
            {
                AddApplicationMessage("Skupina s tímto ID neexistuje", MessageSeverity.Danger);
                return RedirectToAction("Index", "Competitions");
            }

            var fixtures = unitOfWork.FixtureRepository.Get(filter: f => f.Group == groupid).GroupBy(f => f.Round);
            var model = new TeamListViewModel()
            {
                fixtures = fixtures,
                group = unitOfWork.GroupRepository.GetByID(groupid)
            };
            return View(model);
        }

        [RoleAuthorize(Roles = "Admin,SuperAdmin")]
        public ActionResult Delete(int? groupid)
        {
            Groups group = unitOfWork.GroupRepository.GetByID((int)groupid);
            if (group == null)
            {
                AddApplicationMessage("Skupina s tímto ID neexistuje", MessageSeverity.Danger);
                return RedirectToAction("Index", "Competitions");
            }
            var matches = unitOfWork.MatchRepository.Get(filter: m => m.Competition == groupid);
            if (matches.Count() > 0)
            {
                AddApplicationMessage("Skupina již má odehrané zápasy, rozlosování není možné smazat.",MessageSeverity.Danger);
            }
            else
            {
                var fixtures = unitOfWork.FixtureRepository.Get(filter: f => f.Group == groupid);
                foreach (var fixture in fixtures)
                    unitOfWork.FixtureRepository.Delete(fixture);
                unitOfWork.Save();
                AddApplicationMessage("Rozlosování bylo smazáno", MessageSeverity.Success);
            }

            return RedirectToAction("Admin", "Group", new { groupid = groupid });
        }

        [RoleAuthorize(Roles = "Admin, SuperAdmin")]
        public ActionResult GenerateFixtures(int? groupid)
        {
            Groups group = unitOfWork.GroupRepository.GetByID((int)groupid);
            if (group == null)
            {
                AddApplicationMessage("Skupina s tímto ID neexistuje", MessageSeverity.Danger);
                return RedirectToAction("Index", "Competitions");
            }

            int fixexist = unitOfWork.FixtureRepository.Get(filter: f => f.Group == groupid).Count();
            if (fixexist > 0)
            {
                AddApplicationMessage("Rozlosování pro tuto skupinu již existuje", Models.MessageSeverity.Danger);
                return RedirectToAction("Admin", "Group", new { groupid = groupid });
            }

            var teams = unitOfWork.TeamRepository.GetTeamsByGroup((int)groupid).ToList();
            // Random poradi tymu pro rozlosovani
            int n = teams.Count;
            Random rng = new Random();
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Teams value = teams[k];
                teams[k] = teams[n];
                teams[n] = value;
            }

            if (teams.Count % 2 != 0)
            {
                var evenTeam = new Teams(){
                    ID = 0
                };
                teams.Add(evenTeam);
            }
            var games = ListMatches(teams);
            int currentRound = 1;
            foreach (var round in games)
            {
                foreach(var game in round)
                {
                    if (game.Item1.ID == 0 || game.Item2.ID == 0)
                        continue;

                    Fixtures fixture = new Fixtures()
                    {
                        Team1 = game.Item1.ID,
                        Team2 = game.Item2.ID,
                        Round = currentRound,
                        Group = (int)groupid
                    };
                    unitOfWork.FixtureRepository.Insert(fixture);
                }
                currentRound++;
            }
            unitOfWork.Save();
            AddApplicationMessage("Rozlosování skupiny " + group.Name + " bylo provedeno", MessageSeverity.Success);

            return RedirectToAction("Admin","Group", new { groupid = groupid});
        }

        public static List<List<Tuple<Teams, Teams>>> ListMatches(List<Teams> listTeam)
        {
            var result = new List<List<Tuple<Teams, Teams>>>();

            int numDays = (listTeam.Count - 1);
            int halfSize = listTeam.Count / 2;
            var teams = new List<Teams>();
            teams.AddRange(listTeam.Skip(halfSize).Take(halfSize));
            teams.AddRange(listTeam.Skip(1).Take(halfSize - 1).ToArray().Reverse());
            int teamsSize = teams.Count;

            for (int day = 0; day < numDays; day++)
            {
                var round = new List<Tuple<Teams, Teams>>();
                int teamIdx = day % teamsSize;
                round.Add(new Tuple<Teams, Teams>(teams[teamIdx], listTeam[0]));

                for (int idx = 1; idx < halfSize; idx++)
                {
                    int firstTeam = (day + idx) % teamsSize;
                    int secondTeam = (day + teamsSize - idx) % teamsSize;

                    round.Add(new Tuple<Teams, Teams>(teams[firstTeam], teams[secondTeam]));
                }
                result.Add(round);
            }
            return result;
        }
    }
}