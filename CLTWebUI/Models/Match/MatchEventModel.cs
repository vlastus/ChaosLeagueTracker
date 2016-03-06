using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CLT.Data;

namespace CLTWebUI.Models.Match
{
    public class MatchEventModel
    {
        public List<EventDetailModel> events { get; set; }
        public List<Players> players { get; set; }
        public List<Players> players2 { get; set; }
        public Fixtures fixture { get; set; }
        public int eventNo { get; set; }

        public class EventDetailModel
        {
            public int eventNo { get; set; }

            [Display(Name = "Hráč který spustil událost")]
            public int sourcePlayer { get; set; }

            public int sourcePlayerTeam { get; set; }

            [Display(Name = "Hráč byl cílem události")]
            public int targetPlayer { get; set; }

            public int targetPlayerTeam { get; set; }

            [Display(Name = "Výsledek hodu na zranění")]
            public int result { get; set; }

            public MatchEventsTypes type { get; set; }
            public Boolean canceled { get; set; }
        }
    }
}