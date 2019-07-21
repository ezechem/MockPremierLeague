using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MockPremierLeague.API.Dtos
{
    public class FixtureDto
    {
        [Required]
        public int HomeTeam { get; set; }
        [Required]
        public int AwayTeam { get; set; }
        [Required]
        public string MatchCode { get; set; }
        [Required]
        public string Staduim { get; set; }
        [Required]
        public DateTime MatchDate { get; set; }
        [Required]
        public TimeSpan MatchTime { get; set; }
        public bool Status { get; set; }
    }
}
