using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MockPremierLeague.API.Contracts;
using MockPremierLeague.API.Data;
using MockPremierLeague.API.Dtos;
using MockPremierLeague.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MockPremierLeague.API.Services
{
    public class FixtureRepository : IFixtureRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public FixtureRepository( AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<Fixture> CreateFixture(FixtureDto fixtureDto)
        {
            var newFixture= _mapper.Map<Fixture>(fixtureDto);
            newFixture.FixtureURL = Guid.NewGuid().ToString();
            _appDbContext.Add<Fixture>(newFixture);
            if (await _appDbContext.SaveChangesAsync() > 0)
            {
                return newFixture;
            }
            return null;
        }
        public async Task<Fixture> UpdateFixture(FixtureDto fixtureDto, int id)
        {
            var fixtureToUpdate = await GetFixtureById(id);
            if (fixtureToUpdate != null)
            {
                //Map to Existing Fixture
                _mapper.Map(fixtureDto, fixtureToUpdate);
                if (await _appDbContext.SaveChangesAsync() > 0)
                {
                    return fixtureToUpdate;
                }
                return null;
            }
            return null;
        }


        public async Task<bool> DeleteFixture(int id)
        {
            var fixtureToDelete = await GetFixtureById(id);
            if (fixtureToDelete != null)
            {
                _appDbContext.Remove(fixtureToDelete);
                if (await _appDbContext.SaveChangesAsync() > 0)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public async Task<List<Fixture>> GetAllFixture()
        {
            var fixtures = await _appDbContext.Fixtures
                .Include(h=>h.HomeTeam)
                .Include(a=>a.AwayTeam)
                .ToListAsync();
            if (fixtures != null)
            {
                return fixtures;
            }
            return null;
        }

        public async Task<List<Fixture>> GetAllCompletedFixture()
        {
            var fixtures = await _appDbContext.Fixtures
                .Include(h => h.HomeTeam)
                .Include(a => a.AwayTeam)
                .Where(f=>f.Status==true)
                .ToListAsync();
            if (fixtures != null)
            {
                return fixtures;
            }
            return null;
        }

        public async Task<List<Fixture>> GetAllPendingFixture()
        {
            var fixtures = await _appDbContext.Fixtures
               .Include(h => h.HomeTeam)
               .Include(a => a.AwayTeam)
               .Where(f => f.Status == false)
               .ToListAsync();
            if (fixtures != null)
            {
                return fixtures;
            }
            return null;
        }

        public Task<Fixture> GetFixtureByURL(string uniqueURL)
        {
            throw new NotImplementedException();
        }

        public async Task<Fixture> GetFixtureById(int id)
        {
            var fixtureDetails = await _appDbContext.Fixtures
                .Include(h => h.HomeTeam)
                .Include(a => a.AwayTeam)
                .Where(x => x.Id == id).FirstOrDefaultAsync();
            return fixtureDetails;
        }
        
        public async Task<List<Fixture>> SearchFixture(object searchParams)
        {
            List<Fixture> foundFixtures = new List<Fixture>();
            DateTime searchDate;
            TimeSpan searchTime;

            if (DateTime.TryParse((string)searchParams, out searchDate))
            {
                foundFixtures = await _appDbContext.Fixtures
               .Include(h => h.HomeTeam)
               .Include(a => a.AwayTeam)
               .Where(f => f.MatchDate.Date == searchDate.Date)
               .ToListAsync();
            }
            else if (TimeSpan.TryParse((string)searchParams, out searchTime))
            {
                foundFixtures = await _appDbContext.Fixtures
               .Include(h => h.HomeTeam)
               .Include(a => a.AwayTeam)
               .Where(f => f.MatchTime == searchTime)
               .ToListAsync();
            }
            else
            {
               foundFixtures = await _appDbContext.Fixtures
              .Include(h => h.HomeTeam)
              .Include(a => a.AwayTeam)
              .Where(f => f.HomeTeam.Name.Contains((string)searchParams)
              || f.AwayTeam.Name.Contains((string)searchParams)
              || f.MatchCode.Contains((string)searchParams)
              || f.FixtureURL.Contains((string)searchParams)
              || f.Staduim.Contains((string)searchParams))
              .ToListAsync();
            }

            if (foundFixtures != null)
                return foundFixtures;

            return null;           
        }

        public async Task<bool> ValidateFixture(int homeTeamId, int awayTeamId)
        {
            var fixtureExist = await _appDbContext.Fixtures.AnyAsync(x => x.HomeTeamId == homeTeamId && x.AwayTeamId == awayTeamId);
            return fixtureExist;
        }

        public async Task<bool> ValidateFixture(int homeTeamId, int awayTeamId, int id)
        {
            var fixtureExist = await _appDbContext.Fixtures.AnyAsync(x => x.HomeTeamId == homeTeamId && x.AwayTeamId == awayTeamId && x.Id != id);
            return fixtureExist;
        }

       
    }
}
