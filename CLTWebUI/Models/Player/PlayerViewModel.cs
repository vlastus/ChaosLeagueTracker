using CLT.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CLTWebUI.Models.Player
{
    public class PlayerViewModel
    {
        public Players player { get; set; }

        public IEnumerable<SelectListItem> skills { get; set; }

        public int? roll1 { get; set; }
        public int? roll2 { get; set; }
    }
}