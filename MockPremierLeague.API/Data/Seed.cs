using Microsoft.AspNetCore.Identity;
using MockPremierLeague.API.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MockPremierLeague.API.Data
{
    public class Seed
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly AppDbContext _appDbContext;

        public Seed(UserManager<User> userManager, RoleManager<Role> roleManager, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _appDbContext = appDbContext;
        }

        public void SeedData()
        {
            if (!_userManager.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("Data/JSON/users.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);
                var roles = new List<Role>
                {
                    new Role{Name = "User"},
                    new Role{Name = "Admin"}
                };

                foreach (var role in roles)
                {
                    _roleManager.CreateAsync(role).Wait();
                }

                foreach (var user in users)
                {
                    _userManager.CreateAsync(user, "user@2019").Wait();
                    _userManager.AddToRoleAsync(user,"User").Wait();
                }

                var adminUser = new User
                {
                    UserName = "admin@mockpremierleague.com",
                    IsActive=true
                };

                IdentityResult result = _userManager.CreateAsync(adminUser, "admin@2019").Result;

                if (result.Succeeded)
                {
                    var admin = _userManager.FindByNameAsync("admin@mockpremierleague.com").Result;
                    _userManager.AddToRolesAsync(admin, new[] { "Admin"}).Wait();
                }
            }
            if (!_appDbContext.Teams.Any())
            {
                var teamData = System.IO.File.ReadAllText("Data/JSON/team.json");
                var teams = JsonConvert.DeserializeObject<List<Team>>(teamData);

                _appDbContext.AddRange(teams);
            }           
            _appDbContext.SaveChanges();
            if (!_appDbContext.Fixtures.Any())
            {
                var fixtureData = System.IO.File.ReadAllText("Data/JSON/fixtures.json");
                var fixtures = JsonConvert.DeserializeObject<List<Fixture>>(fixtureData);

                _appDbContext.AddRange(fixtures);
            }
            _appDbContext.SaveChanges();

        }
    }
}
