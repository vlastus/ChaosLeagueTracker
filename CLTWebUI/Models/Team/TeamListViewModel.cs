using CLT.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CLTWebUI.Models.Team
{
    public class TeamListViewModel
    {
        public Groups group { get; set; }
        public List<Teams> teams { get; set; }
        public List<GroupTables> table { get; set; }
        public List<Teams> teams2join { get; set; }

        public IEnumerable<IGrouping<int?, Fixtures>> fixtures { get; set; }
    }
}