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
        public static Dictionary<int, string> CasualtyResult = new Dictionary<int, string>
        {
            {1, "11-38 Badly hurt"},
            {2, "41-48 Miss next game" },
            {3, "51-52 Niggling injury" },
            {4, "53-54 -1 MA"},
            {5, "55-56 -1 AV"},
            {6, "57 -1 AG"},
            {7, "58 -1 STR"},
            {8, "61-68 Dead!"}
        };
    }
}