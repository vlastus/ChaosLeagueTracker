using CLT.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CLTWebUI.Models.Match
{
    public class MatchViewModel
    {
        public Fixtures fixture { get; set; }

        public Matches match { get; set; }

        [Display(Name = "Datum zápasu")]
        [Required(ErrorMessage = "Je třeba uvést datum zápasu")]
        public DateTime matchDate { get; set; }

        public int gate1 { get; set; }
        public int gate2 { get; set; }
        public int score1 { get; set; }
        public int score2 { get; set; }
        public int fame1 { get; set; }
        public int fame2 { get; set; }
        public int winning1 { get; set; }
        public int winning2 { get; set; }
        public int fanfactor1 { get; set; }
        public int fanfactor2 { get; set; }
        public int mvp1 { get; set; }
        public int mvp2 { get; set; }
        public int fixtureid { get; set; }
        public List<MatchEventModel.EventDetailModel> events { get; set; }
        public IEnumerable<SelectListItem> players1 { get; set; }
        public IEnumerable<SelectListItem> players2 { get; set; }
        public IEnumerable<SelectListItem> inducements { get; set; }
        public Inducements[] selectedInducements1 { get; set; }
        public Inducements[] selectedInducements2 { get; set; }
        public IEnumerable<SelectListItem> starplayers { get; set; }
        public int[] selectedStarplayers1 { get; set; }
        public int[] selectedStarplayers2 { get; set; }
    }
}