using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MockPremierLeague.API.Models
{
    public class Fixture
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public string MatchCode { get; set; }
        public string Staduim { get; set; }
        [Column(TypeName = "date")]
        public DateTime MatchDate { get; set; }
        public TimeSpan MatchTime { get; set; }
        public bool Status { get; set; }
        public string FixtureURL { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
    }
}
