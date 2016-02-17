using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CLT.Data;
using CLTWebUI.Models.Team;

namespace CLTWebUI.Controllers
{
    public class PlayerController : Controller
    {
        IUnitOfWork unitOfWork;

        public PlayerController(IUnitOfWork uow)
        {
            unitOfWork = uow;
        }
        // GET: Player
        public ActionResult Add(TeamDetailViewModel model)
        {
            model.Team = unitOfWork.TeamRepository.GetByID(model.TeamId);
            var playerTypes = unitOfWork.PlayerTypeRepository.Get(filter: pt => pt.Race == model.Team.Race).ToList();
            model.playertypes = new SelectList(playerTypes, "ID", "Name");
            var selectedType = playerTypes.First(pt => pt.ID == model.SelectedPlayerTypeId);

            if (selectedType.Value > model.Team.Treasury)
            {
                ModelState.AddModelError("Treasury", "Team nemá dostatek peněz na nákup.");
            }

            if (model.Team.Players.Count(p => p.Type == model.SelectedPlayerTypeId) >= selectedType.Limit)
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

            return RedirectToAction("Detail", "Team", new { teamid = model.TeamId });
        }
    }
}