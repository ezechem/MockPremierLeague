using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MockPremierLeague.API.Dtos
{

    public class UserForRegistrationDto
    {
        [Required]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Username")]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "You must specify a password between 6 and 12 characters")]
        public string Password { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime LastActive { get; set; }
        public bool IsActive { get; set; }

        [Required]
        public List<string> Roles { get; set; }

        public UserForRegistrationDto()
        {
            DateCreated = DateTime.Now;
            LastActive = DateTime.Now;
        }
    }
}
