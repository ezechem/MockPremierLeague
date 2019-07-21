using MockPremierLeague.API.Contracts;
using MockPremierLeague.API.Dtos;
using MockPremierLeague.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MockPremierLeague.API.Services
{
    public class AdminRepository : IAdminRepository
    {
        public Task<Fixture> CreateFixture(FixtureDto teamDto)
        {
            throw new NotImplementedException();
        }

        public Task<Team> CreateTeam(TeamDto teamDto)
        {
            throw new NotImplementedException();
        }

        public Task<Fixture> DeleteFixture(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Team> DeleteTeam(int id)
        {
            throw new NotImplementedException();
        }

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

        public Task<Fixture> UpdateFixture(FixtureDto teamDto, int id)
        {
            throw new NotImplementedException();
        }

        public Task<Team> UpdateTeam(TeamDto teamDto, int id)
        {
            throw new NotImplementedException();
        }
    }
}
