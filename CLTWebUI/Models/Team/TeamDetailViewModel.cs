using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CLT.Data;
using System.Web.Mvc;

namespace CLTWebUI.Models.Team
{
    public class TeamDetailViewModel
    {
        public Teams Team { get; set; }
        public IEnumerable<SelectListItem> playertypes { get; set; }

        public int SelectedPlayerTypeId { get; set; }

    }
}