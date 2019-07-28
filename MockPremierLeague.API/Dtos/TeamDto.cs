using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MockPremierLeague.API.Dtos
{
    public class TeamDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string CoachName { get; set; }
        [Required]
        public long YearFounded { get; set; }
        [Required]
        public string Stadium { get; set; }
    }
}
