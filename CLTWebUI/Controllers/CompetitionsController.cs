using CLT.Data;
using CLTWebUI.Models;
using CLTWebUI.Models.Competitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CLTWebUI.Controllers
{
    public class CompetitionsController : BaseController
    {
        IUnitOfWork unitOfWork;
        public CompetitionsController(IUnitOfWork uow)
        {
            unitOfWork = uow;
        }

        // GET: Competitions
        public ActionResult Index()
        {
            List<Competitions> comps = unitOfWork.CompetitionRepository.Get(includeProperties: "Groups").ToList();
            comps.Reverse();
            var model = new CompetitionsViewModel()
            {
                Competitions = comps
            };

            return View(model);
        }

        [RoleAuthorize(Roles = "Admin, SuperAdmin")]
        public ActionResult Admin()
        {
            List<Competitions> comps = unitOfWork.CompetitionRepository.Get(includeProperties: "Groups").ToList();
            comps.Reverse();
            var model = new CompetitionsViewModel()
            {
                Competitions = comps
            };
            return View(model);
        }

        [HttpGet]
        [RoleAuthorize(Roles = "Admin, SuperAdmin")]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [RoleAuthorize(Roles = "Admin, SuperAdmin")]
        public ActionResult Add(AddCompetitionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var comp = new Competitions()
                {
                    Name = model.name,
                    Status = Status.Active
                };

                unitOfWork.CompetitionRepository.Insert(comp);
                unitOfWork.Save();
                AddApplicationMessage("Soutěž byla úspešně založena", MessageSeverity.Success);

                return RedirectToAction("Admin", "Competitions");
            }
            AddApplicationMessage("Nepodařilo se založit soutěž, zkontrolujte formulář", MessageSeverity.Danger);
            return View(model);
        }

        [HttpGet]
        [RoleAuthorize(Roles = "Admin, SuperAdmin")]
        public ActionResult Edit(int? compid)
        {
            if (compid == null)
            {
                AddApplicationMessage("Nebyl uveden identifikátor soutěže", MessageSeverity.Danger);
                return RedirectToAction("Admin", "Competitions");
            }

            Competitions comp = unitOfWork.CompetitionRepository.GetByID(compid);
            if (comp == null)
            {
                AddApplicationMessage("Id neodpovídá žádné soutěži", MessageSeverity.Danger);
                return RedirectToAction("Admin", "Competitions");
            }

            var model = new AddCompetitionViewModel()
            {
                compid = compid,
                name = comp.Name
            };
            return View(model);
        }

        [HttpPost]
        [RoleAuthorize(Roles = "Admin, SuperAdmin")]
        public ActionResult Edit(AddCompetitionViewModel model)
        {
            if (ModelState.IsValid && model.compid != null)
            {
                var comp = unitOfWork.CompetitionRepository.GetByID(model.compid);
                if (comp != null)
                {
                    comp.Name = model.name;

                    unitOfWork.CompetitionRepository.Update(comp);
                    unitOfWork.Save();
                    AddApplicationMessage("Soutěž byla úspešně změněna", MessageSeverity.Success);

                    return RedirectToAction("Admin", "Competitions");
                }
            }
            AddApplicationMessage("Nepodařilo se uložit soutěž, zkontrolujte formulář", MessageSeverity.Danger);
            return View(model);
        }

        [RoleAuthorize(Roles = "Admin, SuperAdmin")]
        public ActionResult Delete(int? compid)
        {
            if (compid == null)
                AddApplicationMessage("Nebyl uveden identifikátor soutěže", MessageSeverity.Danger);
            else
            {
                var comp = unitOfWork.CompetitionRepository.GetByID(compid);
                if (comp == null)
                {
                    AddApplicationMessage("Id neodpovídá žádné soutěži", MessageSeverity.Danger);
                }
                else
                {
                    comp.Status = Status.Inactive;

                    unitOfWork.CompetitionRepository.Update(comp);
                    unitOfWork.Save();
                    AddApplicationMessage("Soutěž byla ukončena", MessageSeverity.Success);
                }
            }
            return RedirectToAction("Admin", "Competitions");
        }

        [HttpGet]
        [RoleAuthorize(Roles = "Admin, SuperAdmin")]
        public ActionResult AddGroup(int? compid)
        {
            if (compid == null)
            {
                AddApplicationMessage("Nebyl uveden identifikátor soutěže", MessageSeverity.Danger);
                return RedirectToAction("Admin", "Competitions");
            }

            var comp = unitOfWork.CompetitionRepository.GetByID(compid);
            if (comp == null)
            {
                AddApplicationMessage("Id neodpovídá žádné soutěži", MessageSeverity.Danger);
                return RedirectToAction("Admin", "Competitions");
            }

            var model = new AddGroupViewModel()
            {
                competition = comp
            };

            return View(model);
        }

        [HttpPost]
        [RoleAuthorize(Roles = "Admin, SuperAdmin")]
        public ActionResult AddGroup(AddGroupViewModel model)
        {
            model.competition = unitOfWork.CompetitionRepository.GetByID(model.compid);
            if(ModelState.IsValid)
            {
                var group = new Groups()
                {
                    Competition = model.compid,
                    Name = model.name,
                    Playoff = model.playoff ? 1 : 0
                };

                unitOfWork.GroupRepository.Insert(group);
                unitOfWork.Save();
                AddApplicationMessage("Skupina byla přidána", MessageSeverity.Success);

                return RedirectToAction("Admin","Competitions");
            }

            AddApplicationMessage("Skupinu se nepodařilo uložit, zkontrolujte formulář", MessageSeverity.Danger);
            return View(model);
        }

        [HttpGet]
        [RoleAuthorize(Roles = "Admin, SuperAdmin")]
        public ActionResult EditGroup(int? groupid)
        {
            if (groupid == null)
            {
                AddApplicationMessage("Nebylo uvedeno ID skupiny", MessageSeverity.Danger);
                return RedirectToAction("Admin", "Competitions");
            }

            var group = unitOfWork.GroupRepository.GetByID(groupid);
            if (group == null)
            {
                AddApplicationMessage("Skupina nebyla nalezena", MessageSeverity.Danger);
                return RedirectToAction("Admin", "Competitions");
            }

            var model = new AddGroupViewModel()
            {
                groupid = (int)groupid,
                name = group.Name,
                playoff = group.Playoff == 1 ? true : false,
                compid = group.Competition,
                competition = group.Competitions
            };
            return View("AddGroup", model);
        }

        [HttpPost]
        [RoleAuthorize(Roles = "Admin, SuperAdmin")]
        public ActionResult EditGroup(AddGroupViewModel model)
        {
            var group = unitOfWork.GroupRepository.GetByID(model.groupid);
            if (group == null)
            {
                AddApplicationMessage("Skupina nebyla nalezena", MessageSeverity.Danger);
            }
            else
            {
                group.Name = model.name;
                group.Playoff = model.playoff ? 1 : 0;

                unitOfWork.GroupRepository.Update(group);
                unitOfWork.Save();
                AddApplicationMessage("Skupina byla upravena", MessageSeverity.Success);
            }

            model.competition = group.Competitions;

            return RedirectToAction("Admin", "Competitions");
        }
    }
}