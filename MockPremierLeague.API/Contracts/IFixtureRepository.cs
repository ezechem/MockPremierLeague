using MockPremierLeague.API.Dtos;
using MockPremierLeague.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MockPremierLeague.API.Contracts
{
    public interface IFixtureRepository
    {
        //Manage Fixtures
        Task<Fixture> CreateFixture(FixtureDto fixtureDto);
        Task<Fixture> UpdateFixture(FixtureDto fixtureDto, int id);
        Task<bool> DeleteFixture(int id);
        Task<List<Fixture>> GetAllFixture();
        Task<List<Fixture>> GetAllCompletedFixture();
        Task<List<Fixture>> GetAllPendingFixture();
        Task<List<Fixture>> SearchFixture(object searchParams);
        Task<Fixture> GetFixtureById(int id);
        Task<Fixture> GetFixtureByURL(string uniqueURL);
        Task<bool> ValidateFixture(int homeTeamId, int awayTeamId);
        Task<bool> ValidateFixture(int homeTeamId, int awayTeamId, int id);
    }
}
