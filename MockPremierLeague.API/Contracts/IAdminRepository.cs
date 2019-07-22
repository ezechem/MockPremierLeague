using MockPremierLeague.API.Dtos;
using MockPremierLeague.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MockPremierLeague.API.Contracts
{
    public interface IAdminRepository
    {
        //Manage Team
        Task<Team> CreateTeam(TeamDto teamDto);
        Task<Team> UpdateTeam(TeamDto teamToUpdate, int id);
        Task<bool> DeleteTeam(int id);
        Task<List<Team>> GetAllTeams();
        Task<Team> GetTeamById(int id);
        Task<bool> ValidateTeam(string teamName);
        Task<bool> ValidateTeam(string teamName, int id);

        //Manage Fixtures
        Task<Fixture> CreateFixture(FixtureDto teamDto);
        Task<Fixture> UpdateFixture(FixtureDto teamDto, int id);
        Task<bool> DeleteFixture(int id);
        Task<List<Fixture>> GetAllFixture();
        Task<Fixture> GetFixtureByURL(string uniqueURL);
    }
}
