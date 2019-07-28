using MockPremierLeague.API.Data;
using MockPremierLeague.API.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MockPremierLeague.Test
{
    public static class DbContextExtensions
    {
        public static void Seed(this AppDbContext dbContext)
        {
            // Add entities for DbContext instance

            dbContext.Teams.Add(new Team
            {
                Name = "Arsenal",
                Code = "ARS",
                Address = "Arsenal",
                CoachName = "Unai Emery",
                YearFounded = 1886,
                Stadium = "Emirates Stadium"
            });

            dbContext.Teams.Add(new Team
            {
                Name = "Bournemouth",
                Code = "BOU",
                Address = "Bournemouth",
                CoachName = "Eddie Howe",
                YearFounded = 1899,
                Stadium = "Vitality Stadium"
            });

            dbContext.Teams.Add(new Team
            {
                Name = "Brighton & Hove Albion",
                Code = "BHA",
                Address = "Brighton",
                CoachName = "Chris Hughton",
                YearFounded = 1901,
                Stadium = "Amex Stadium"
            });

            dbContext.Teams.Add(new Team
            {
                Name = "Burnley",
                Code = "BUR",
                Address = "Burnley",
                CoachName = "Sean Dyche",
                YearFounded = 1882,
                Stadium = "Turf Moor"
            });

            dbContext.Teams.Add(new Team
            {
                Name = "Cardiff City",
                Code = "CAR",
                Address = "Cardiff",
                CoachName = "Ole Gunnar Solskjær",
                YearFounded = 1899,
                Stadium = "Neil Warnock"
            });

            dbContext.Teams.Add(new Team
            {
                Name = "Chelsea",
                Code = "CHE",
                Address = "Chelsea",
                CoachName = "Frank Lampard",
                YearFounded = 1905,
                Stadium = "Stamford Bridge"
            });
        }
    }
}
