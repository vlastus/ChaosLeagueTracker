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
        [Display(Name = "Vlastník týmu")]
        [Required]
        public int owner { get; set; }

        [Display(Name = "Rasa")]
        [Required]
        public int race { get; set; }

        [Display(Name = "Počáteční hodnota")]
        public int startValue { get; set; }

        [Display(Name = "Jméno týmu")]
        [Required(ErrorMessage = "Je třeba zadat jméno týmu")]
        public string name { get; set; }
        public IEnumerable<SelectListItem> users { get; set; }
        public IEnumerable<SelectListItem> races { get; set; }
    }
}