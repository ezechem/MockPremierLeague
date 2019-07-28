using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MockPremierLeague.API.Models
{
    public class Team
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string  Code { get; set; }
        public string Address { get; set; }
        public string CoachName { get; set; }
        public string Stadium { get; set; }
        public long YearFounded { get; set; }
    }
}
