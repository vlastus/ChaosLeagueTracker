using CLT.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CLTWebUI.Models.Team
{
    public class AddTeamViewModel
    {
        public Teams team { get; set; }

        [Display(Name = "Vlastník týmu")]
        [Required]
        public int owner { get; set; }

        [Display(Name = "Rasa")]
        [Required]
        public int race { get; set; }

        [Display(Name = "Počáteční hodnota")]
        public int? startValue { get; set; }

        [Display(Name = "Jméno týmu")]
        [Required(ErrorMessage = "Je třeba zadat jméno týmu")]
        [StringLength(50, ErrorMessage = "Jméno může mít maximálně {1} znaků")]
        public string name { get; set; }

        [Display(Name = "Rerolly")]
        public int rerolls { get; set; }

        [Display(Name = "Fan faktor")]
        public int fanfactor { get; set; }

        [Display(Name = "Assistant coaches")]
        public int asscoaches { get; set; }

        [Display(Name = "Cheerleaders")]
        public int cheerleaders { get; set; }

        [Display(Name = "Apothecary(0 ne,1 ano)")]
        public int apothecary { get; set; }

        [Display(Name = "Team Value")]
        public int value { get; set; }

        [Display(Name = "Treasury")]
        public int treasury { get; set; }

        public int teamid { get; set; }

        public IEnumerable<SelectListItem> users { get; set; }
        public IEnumerable<SelectListItem> races { get; set; }
    }
}