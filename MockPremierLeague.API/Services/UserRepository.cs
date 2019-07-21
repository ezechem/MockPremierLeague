using MockPremierLeague.API.Contracts;
using MockPremierLeague.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MockPremierLeague.API.Services
{
    public class UserRepository : IUserRepository

    {
        public Task<List<Fixture>> GetAllFixture()
        {
            throw new NotImplementedException();
        }

        public Task<List<Team>> GetAllTeams()
        {
            throw new NotImplementedException();
        }

        public Task<Fixture> GetFixtureByURL(string uniqueURL)
        {
            throw new NotImplementedException();
        }

        public Task<Team> GetTeamById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Fixture>> SearchFixture(string fixtureParam)
        {
            throw new NotImplementedException();
        }

        public Task<List<Team>> SearchTeam(string teamParam)
        {
            throw new NotImplementedException();
        }
    }
}
