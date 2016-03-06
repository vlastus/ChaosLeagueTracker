using CLT.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CLTWebUI.Models.Match
{
    public class MatchViewModel
    {
        public Fixtures fixture { get; set; }
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
        public List<MatchEventModel.EventDetailModel> events { get; set; }
    }
}