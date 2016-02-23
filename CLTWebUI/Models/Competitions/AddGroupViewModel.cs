using CLT.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CLTWebUI.Models.Competitions
{
    public class AddGroupViewModel
    {
        [Required(ErrorMessage = "Je třeba uvést název skupiny")]
        [Display(Name = "Název skupiny")]
        public string name { get; set; }

        [Display(Name = "PlayOff")]
        public Boolean playoff { get; set; }

        public int compid { get; set; }
        public int? groupid { get; set; }

        public CLT.Data.Competitions competition { get; set; }
    }
}