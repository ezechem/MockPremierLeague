using MockPremierLeague.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MockPremierLeague.API.Contracts
{
    public interface IUserRepository
    {
        Task<List<Team>> GetAllTeams();
        Task<List<Fixture>> GetAllFixture();
        Task<Team> GetTeamById(int id);
        Task<Fixture> GetFixtureByURL(string uniqueURL);
        Task<List<Fixture>> SearchFixture(string fixtureParam);
        Task<List<Team>> SearchTeam(string teamParam);
    }
}
