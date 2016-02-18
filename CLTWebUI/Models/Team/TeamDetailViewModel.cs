using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CLT.Data;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CLTWebUI.Models.Team
{
    public class TeamDetailViewModel
    {
        public Teams Team { get; set; }

        public IEnumerable<SelectListItem> playertypes { get; set; }

        [Required]
        [Display(Name="Typ hráče")]
        public int SelectedPlayerTypeId { get; set; }

        [Required(ErrorMessage ="Pole Jméno hráče je nutné vyplnit")]
        [StringLength(50, ErrorMessage ="Jméno může mít maximálně {1} znaků")]
        [Display(Name = "Jméno")]
        public string NewPlayerName {get; set; }

        public int TeamId { get; set; }

        public Boolean viewAddPlayerModal { get; set; }
        public List<string> AddNewPlayerErrors { get; set; }
    }
}