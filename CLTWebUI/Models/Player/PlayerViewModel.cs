using CLT.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CLTWebUI.Models.Player
{
    public class PlayerViewModel
    {
        public Players player { get; set; }

        public IEnumerable<SelectListItem> skills { get; set; }

        [Display( Name = "Nový skill")]
        public int SelectedSkillID { get; set; }

        public int? roll1 { get; set; }
        public int? roll2 { get; set; }
        public string selectStat { get; set; }
        [Required]
        public string selectLUMode { get; set; }
        [Required]
        public int playerid { get; set; }
    }
}