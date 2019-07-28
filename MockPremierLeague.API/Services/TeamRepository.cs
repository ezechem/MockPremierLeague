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
    public class TeamRepository : ITeamRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public TeamRepository( AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
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

        public async Task<List<Team>> GetAllTeams()
        {
            var teams = await _appDbContext.Teams.ToListAsync();
            if (teams != null)
            {
                return teams;
            }
            return null;
        }
       
        public async Task<Team> GetTeamById(int id)
        {
            var teamDetails = await _appDbContext.Teams.Where(x => x.Id == id).FirstOrDefaultAsync();
            return teamDetails;
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

        public async Task<List<Team>> SearchTeam(object searchParams)
        {
            List<Team> foundTeam = new List<Team>();
            long yearFounded;


            if (long.TryParse((string)searchParams, out yearFounded))
            {
                foundTeam = await _appDbContext.Teams
                    .Where(t => t.YearFounded == yearFounded)
               .ToListAsync();
            }
            else
            {
                foundTeam = await _appDbContext.Teams
                    .Where(f => f.Name.Contains((string)searchParams)
                    || f.Code.Contains((string)searchParams)
                    || f.Address.Contains((string)searchParams)
                    || f.CoachName.Contains((string)searchParams)
                    || f.Stadium.Contains((string)searchParams))
                    .ToListAsync();
            }

            if (foundTeam != null)
                return foundTeam;

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
