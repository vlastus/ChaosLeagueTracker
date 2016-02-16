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

        [Required]
        [StringLength(50)]
        [Display(Name = "Jméno")]
        public string NewPlayerName {get; set; }

    }
}