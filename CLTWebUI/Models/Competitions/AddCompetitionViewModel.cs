using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CLTWebUI.Models.Competitions
{
    public class AddCompetitionViewModel
    {
        [Required(ErrorMessage = "Je třeba uvést název soutěže")]
        [Display(Name = "Název soutěže")]
        public string name { get; set; }

        public int? compid { get; set; }
    }
}