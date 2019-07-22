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
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public AdminRepository( AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }


        public Task<Fixture> CreateFixture(FixtureDto teamDto)
        {
            throw new NotImplementedException();
        }

        public async Task<Team> CreateTeam(TeamDto teamDto)
        {
            var newTeam = _mapper.Map<Team>(teamDto);
            _appDbContext.Add<Team>(newTeam);

            if (await _appDbContext.SaveChangesAsync() > 0)
            {
                return newTeam;
            }
            return null;
        }

        public async Task<bool> DeleteFixture(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteTeam(int id)
        {
            var teamToDelete = await GetTeamById(id);
            if (teamToDelete != null)
            {
                _appDbContext.Remove(teamToDelete);
                if (await _appDbContext.SaveChangesAsync() > 0)
                {
                    return true;
                }
                return false;
            }
            return false;
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

        public async Task<Team> GetTeamById(int id)
        {
            var teamDetails = await _appDbContext.Teams.Where(x => x.Id == id).FirstOrDefaultAsync();
            return teamDetails;
        }

        public Task<Fixture> UpdateFixture(FixtureDto teamDto, int id)
        {
            throw new NotImplementedException();
        }

       public async Task<Team> UpdateTeam(TeamDto teamToUpdate, int id)
        {
            var updatedTeam = await GetTeamById(id);
            if (updatedTeam != null)
            {
                //Map to Existing team
                _mapper.Map(teamToUpdate, updatedTeam);
                if (await _appDbContext.SaveChangesAsync() > 0)
                {
                    return updatedTeam;
                }
                return null;
            }
            return null;
        }

        public async Task<bool> ValidateTeam(string teamName)
        {
            var teamExist = await _appDbContext.Teams.AnyAsync(x => x.Name == teamName);
            return teamExist;
        }

        public async Task<bool> ValidateTeam(string teamName, int id)
        {
            var teamExist = await _appDbContext.Teams.AnyAsync(x => x.Name == teamName && x.Id != id);
            return teamExist;
        }

    }
}
