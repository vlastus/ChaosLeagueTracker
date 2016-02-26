using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CLT.Data;
using CLTWebUI.Models.Team;
using CLTWebUI.Models;
using CLTWebUI.Models.Player;

namespace CLTWebUI.Controllers
{
    public class PlayerController : BaseController
    {
        IUnitOfWork unitOfWork;

        public PlayerController(IUnitOfWork uow)
        {
            unitOfWork = uow;
        }
        // GET: Player
        [Authorize]
        public ActionResult Add(TeamDetailViewModel model)
        {
            model.Team = unitOfWork.TeamRepository.GetByID(model.TeamId);
            if (User.Identity.Name != model.Team.Users.Name && User.IsInRole("Common"))
            {
                AddApplicationMessage("Na tuto akci nemáte oprávnění", MessageSeverity.Warning);
                return Redirect(Request.UrlReferrer.ToString());
            }

            var playerTypes = unitOfWork.PlayerTypeRepository.Get(filter: pt => pt.Race == model.Team.Race).ToList();
            model.playertypes = new SelectList(playerTypes, "ID", "Name");
            var selectedType = playerTypes.First(pt => pt.ID == model.SelectedPlayerTypeId);

            if (selectedType.Value > model.Team.Treasury)
            {
                ModelState.AddModelError("Treasury", "Team nemá dostatek peněz na nákup.");
            }

            if (model.Team.Players.Count(p => (p.Type == model.SelectedPlayerTypeId && p.Status == Status.Active)) >= selectedType.Limit)
            {
                ModelState.AddModelError("Limit", "Team dosáhl limitu hráčů tohoto typu v rosteru.");
            }

            if (!ModelState.IsValid)
            {
                model.AddNewPlayerErrors = new List<string>();
                foreach (ModelState modelState in ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        model.AddNewPlayerErrors.Add(error.ErrorMessage);
                    }
                }
                model.viewAddPlayerModal = true;
                return View("../Team/Detail", model);
            }

            var newPlayer = new Players()
            {
                Team = model.Team.ID,
                Type = model.SelectedPlayerTypeId,
                Name = model.NewPlayerName,
                MA = selectedType.MA,
                ST = selectedType.ST,
                AG = selectedType.AG,
                AV = selectedType.AV,
                Value = selectedType.Value,
                CAS = 0,
                COMP = 0,
                INT = 0,
                Kills = 0,
                MNG = 0,
                MVP = 0,
                NI = 0,
                SPP = 0,
                TD = 0,
                Journeyman = 0,
                Level = 0,
                Status = Status.Active
            };

            var typeskills = selectedType.Skillset.Split('|').ToList();
            foreach (var ts in typeskills)
                newPlayer.PlayerSkills.Add(new PlayerSkills()
                {
                    Skill = (Skills)Enum.Parse(typeof(Skills), ts),
                    Player = newPlayer.ID
                });
            model.Team.Treasury -= selectedType.Value;
            model.Team.Value += selectedType.Value;

            unitOfWork.TeamRepository.Update(model.Team);
            unitOfWork.PlayerRepository.Insert(newPlayer);
            unitOfWork.Save();
            AddApplicationMessage("Hráč byl přidán", MessageSeverity.Success);

            return RedirectToAction("Detail", "Team", new { teamid = model.TeamId });
        }

        [Authorize]
        public ActionResult Delete(int? playerId)
        {
            if (playerId == null)
            {
                AddApplicationMessage("Neudáno ID hráče", MessageSeverity.Success);
                return Redirect(Request.UrlReferrer.ToString());
            }

            Players player = unitOfWork.PlayerRepository.GetByID(playerId);

            if (player == null)
            {
                AddApplicationMessage("Hráč nebyl nalezen", MessageSeverity.Success);
                return Redirect(Request.UrlReferrer.ToString());
            }

            Teams team = unitOfWork.TeamRepository.GetByID(player.Team);

            if (User.Identity.Name != team.Users.Name && User.IsInRole("Common"))
            {
                AddApplicationMessage("Na tuto akci nemáte oprávnění", MessageSeverity.Warning);
                return Redirect(Request.UrlReferrer.ToString());
            }

            player.Status = Status.Inactive;
            unitOfWork.PlayerRepository.Update(player);
            team.Value -= player.Value;

            unitOfWork.TeamRepository.Update(team);
            unitOfWork.Save();
            AddApplicationMessage("Hráč byl odstraněn",MessageSeverity.Success);

            return RedirectToAction("Detail", "Team", new { teamid = player.Team });
        }

        [Authorize]
        public ActionResult LevelUp(int? roll1, int? roll2, int? playerId)
        {
            if (playerId == null)
            {
                AddApplicationMessage("Neudáno ID hráče", MessageSeverity.Success);
                return Redirect(Request.UrlReferrer.ToString());
            }

            Players player = unitOfWork.PlayerRepository.GetByID(playerId);

            if (player == null)
            {
                AddApplicationMessage("Hráč nebyl nalezen", MessageSeverity.Success);
                return Redirect(Request.UrlReferrer.ToString());
            }

            var model = new PlayerViewModel()
            {
                player = player,
                roll1 = roll1,
                roll2 = roll2
            };

            var skills = from Skills s in Enum.GetValues(typeof(Skills)) select new { ID = (int)s, Name = s.ToString() };
            var fskills = skills.Except(skills).ToList();
            fskills.Add(new { ID = 0, Name = "-- vyberte skill --" });
                        
            var availablegroups = unitOfWork.TypeGroupRepository.Get(filter: tg => tg.PlayerType == player.PlayerTypes.ID);
            foreach (var agrp in availablegroups)
            {
                foreach(var skill in skills)
                {
                    if (Math.Floor((double)skill.ID/100) == agrp.SkillGroup 
                        && ((agrp.IsDouble == 1 && (roll1 == roll2)) || agrp.IsDouble == 0)
                        && player.PlayerSkills.All(ps => (int)ps.Skill != skill.ID))
                    {
                        fskills.Add(skill);
                    }
                }
            }

            model.skills = new SelectList(fskills, "ID", "Name");

            return View(model);
        }

        public ActionResult LevelUpFinal(PlayerViewModel model)
        {
            var player = unitOfWork.PlayerRepository.GetByID(model.playerid);
            var team = unitOfWork.TeamRepository.GetByID(player.Team);
            Boolean err = false;
            int val = 0;
            switch (model.selectLUMode)
            {
                case "skillmode":
                    var skill = new PlayerSkills()
                    {
                        Player = player.ID,
                        Skill = (Skills)model.SelectedSkillID
                    };
                    val = 20000;
                    foreach (var grp in player.PlayerTypes.TypeGroups)
                        if (grp.SkillGroup == Math.Floor((double)model.SelectedSkillID / 100) && grp.IsDouble == 1)
                            val = 30000;
                    unitOfWork.PlayerSkillRepository.Insert(skill);
                    break;
                case "statmode":
                    switch (model.selectStat)
                    {
                        case "str":
                            player.ST += 1;
                            val = 50000;
                            break;
                        case "agi":
                            player.AG += 1;
                            val = 40000;
                            break;
                        case "arm":
                            player.AV += 1;
                            val = 30000;
                            break;
                        case "mov":
                            player.MA += 1;
                            val = 30000;
                            break;
                        default:
                            AddApplicationMessage("Chyba při level-upu.", MessageSeverity.Danger);
                            err = true;
                            break;
                    }
                    break;
                default:
                    AddApplicationMessage("Chyba při level-upu.", MessageSeverity.Danger);
                    err = true;
                    break;
            }
            if (!err)
            {
                player.Value += val;
                player.Level += 1;
                team.Value += val;

                unitOfWork.TeamRepository.Update(team);
                unitOfWork.PlayerRepository.Update(player);
                unitOfWork.Save();
                AddApplicationMessage("Hráč byl úspěšně odlevlen", MessageSeverity.Success);
            }

            return RedirectToAction("Detail", "Team", new { teamid = player.Team});
        }
    }
}