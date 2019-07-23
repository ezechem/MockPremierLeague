using MockPremierLeague.API.Dtos;
using MockPremierLeague.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MockPremierLeague.API.Contracts
{
    public interface ITeamRepository
    {
        //Manage Team
        Task<Team> CreateTeam(TeamDto teamDto);
        Task<Team> UpdateTeam(TeamDto teamToUpdate, int id);
        Task<bool> DeleteTeam(int id);
        Task<List<Team>> GetAllTeams();
        Task<Team> GetTeamById(int id);
        Task<bool> ValidateTeam(string teamName);
        Task<bool> ValidateTeam(string teamName, int id);
        Task<List<Team>> SearchTeam(object searchParams);
    }
}
