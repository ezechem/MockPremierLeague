using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MockPremierLeague.API.Dtos
{
    public class UserForLoginDto
    {

        [Required]
        [Display(Name = "Username")]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
