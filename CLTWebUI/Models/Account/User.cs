using CLT.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CLTWebUI.Models.Account
{
    public class User
    {
        [Required(ErrorMessage="Je třeba zadat uživatelské jméno")]
        [Display(Name="Uživatelské jméno")]
        public string name { get; set; }

        [Required(ErrorMessage="Je třeba zadat heslo")]
        [Display(Name="Heslo")]
        public string password { get; set; }

        public UserRoles role { get; set; }
    }
}