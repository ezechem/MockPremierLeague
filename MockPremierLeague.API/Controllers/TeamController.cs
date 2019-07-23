using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MockPremierLeague.API.Contracts;
using MockPremierLeague.API.Data;
using MockPremierLeague.API.Dtos;
using MockPremierLeague.API.Enumerators;
using Newtonsoft.Json;

namespace MockPremierLeague.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {

        private readonly AppDbContext _appDbContext;
        private readonly ITeamRepository _teamRepository;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public TeamController(AppDbContext appDbContext, ITeamRepository teamRepository, IConfiguration config, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _teamRepository = teamRepository;
            _config = config;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateTeam(TeamDto teamDto)
        {
            var response = new BaseReturnDto();
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorList = (from item in ModelState.Values
                                     from error in item.Errors
                                     select error.ErrorMessage).ToList();
                    string errorMessage = JsonConvert.SerializeObject(errorList);
                    response.Status = nameof(ResponseCode.Error);
                    response.StatusCode = (int)ResponseCode.Error;
                    response.Message = errorMessage;
                    response.ModelToReturn = teamDto;
                    return Ok(response);
                }

                if (!await _teamRepository.ValidateTeam(teamDto.Name))
                {
                    var createdTeam = await _teamRepository.CreateTeam(teamDto);
                    if (createdTeam != null)
                    {
                        response.Status = nameof(ResponseCode.Success);
                        response.StatusCode = (int)ResponseCode.Success;
                        response.Message = "Team created successfully";
                        response.ModelToReturn = createdTeam;
                        return Ok(response);
                    }
                    else
                    {
                        response.Status = nameof(ResponseCode.Failed);
                        response.StatusCode = (int)ResponseCode.Failed;
                        response.Message = "Team could not be created";
                        response.ModelToReturn = teamDto;
                        return Ok(response);
                    }
                }
                else
                {
                    response.Status = nameof(ResponseCode.Error);
                    response.StatusCode = (int)ResponseCode.Error;
                    response.Message = "Team with same name already exist.";
                    response.ModelToReturn = teamDto;
                    return Ok(response);
                }
            }
            catch (Exception)
            {
                response.Status = nameof(ResponseCode.Error);
                response.StatusCode = (int)ResponseCode.Error;
                response.Message = "Opps!!! An error occured";
                response.ModelToReturn = teamDto;
                return Ok(response);
            }
        }

        [HttpPut("UpdateTeam/{id:int}", Name = "UpdateTeam")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTeam([FromBody]TeamDto teamToUpdateDto, int id)
        {
            var response = new BaseReturnDto();
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorList = (from item in ModelState.Values
                                     from error in item.Errors
                                     select error.ErrorMessage).ToList();
                    string errorMessage = JsonConvert.SerializeObject(errorList);
                    response.Status = nameof(ResponseCode.Error);
                    response.StatusCode = (int)ResponseCode.Error;
                    response.Message = errorMessage;
                    response.ModelToReturn = teamToUpdateDto;
                    return Ok(response);
                }

                if (!await _teamRepository.ValidateTeam(teamToUpdateDto.Name, id))
                {
                    //Update Team
                    var updatedTeam = await _teamRepository.UpdateTeam(teamToUpdateDto, id);
                    if (updatedTeam != null)
                    {
                        response.Status = nameof(ResponseCode.Success);
                        response.StatusCode = (int)ResponseCode.Success;
                        response.Message = "Team updated successfully";
                        response.ModelToReturn = updatedTeam;
                        return Ok(response);
                    }
                    else
                    {
                        response.Status = nameof(ResponseCode.Failed);
                        response.StatusCode = (int)ResponseCode.Failed;
                        response.Message = "Team could not be updated";
                        response.ModelToReturn = teamToUpdateDto;
                        return Ok(response);
                    }
                }
                else
                {
                    response.Status = nameof(ResponseCode.Failed);
                    response.StatusCode = (int)ResponseCode.Failed;
                    response.Message = "A Team with same name already exist.";
                    response.ModelToReturn = teamToUpdateDto;
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                response.Status = nameof(ResponseCode.Error);
                response.StatusCode = (int)ResponseCode.Error;
                response.Message = "Opps!!! An error occured";
                response.ModelToReturn = teamToUpdateDto;
                return Ok(response);
            }
        }

        [HttpDelete("DeleteTeam/{id:int}", Name = "DeleteTeam")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var response = new BaseReturnDto();
            try
            {
                if (id == 0)
                {
                    response.Status = nameof(ResponseCode.Success);
                    response.StatusCode = (int)ResponseCode.Success;
                    response.Message = "No Team specified for deletion";
                    response.ModelToReturn = null;
                    return Ok(response);
                }

                //Delete Team
                if (await _teamRepository.DeleteTeam(id))
                {
                    response.Status = nameof(ResponseCode.Success);
                    response.StatusCode = (int)ResponseCode.Success;
                    response.Message = "Team deleted successfully";
                    response.ModelToReturn = null;
                    return Ok(response);
                }
                else
                {
                    response.Status = nameof(ResponseCode.Failed);
                    response.StatusCode = (int)ResponseCode.Failed;
                    response.Message = "Team could not be deleted";
                    response.ModelToReturn = null;
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                response.Status = nameof(ResponseCode.Error);
                response.StatusCode = (int)ResponseCode.Error;
                response.Message = "Opps!!! An error occured";
                response.ModelToReturn = null;
                return Ok(response);
            }
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetAllTeams()
        {
            var response = new BaseReturnDto();
            try
            {
                var teams = await _teamRepository.GetAllTeams();
                if (teams.Count > 0)
                {
                    response.Status = nameof(ResponseCode.Success);
                    response.StatusCode = (int)ResponseCode.Success;
                    response.Message = "Teams retrived succesfully";
                    response.ModelToReturn = teams;
                    return Ok(response);
                }

                response.Status = nameof(ResponseCode.RecordNotFound);
                response.StatusCode = (int)ResponseCode.RecordNotFound;
                response.Message = "No Team available";
                response.ModelToReturn = null;
                return Ok(response);
            }
            catch (Exception)
            {
                response.Status = nameof(ResponseCode.Error);
                response.StatusCode = (int)ResponseCode.Error;
                response.Message = "Oops!!!, Teams could not be retrieved";
                response.ModelToReturn = null;
                return Ok(response);
            }
        }

        [HttpGet("GetTeamById/{id:int}", Name = "GetTeamById")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetTeamById(int id)
        {
            var response = new BaseReturnDto();
            try
            {
                var teams = await _teamRepository.GetTeamById(id);
                if (teams != null)
                {
                    response.Status = nameof(ResponseCode.Success);
                    response.StatusCode = (int)ResponseCode.Success;
                    response.Message = "Team retrived succesfully";
                    response.ModelToReturn = teams;
                    return Ok(response);
                }

                response.Status = nameof(ResponseCode.RecordNotFound);
                response.StatusCode = (int)ResponseCode.RecordNotFound;
                response.Message = "No such Team available";
                response.ModelToReturn = null;
                return Ok(response);
            }
            catch (Exception)
            {
                response.Status = nameof(ResponseCode.Error);
                response.StatusCode = (int)ResponseCode.Error;
                response.Message = "Oops!!!, Team could not be retrieved";
                response.ModelToReturn = null;
                return Ok(response);
            }
        }

        [HttpPost("SearchTeam", Name = "SearchTeam")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchTeam(string searchParam)
        {
            var response = new BaseReturnDto();
            try
            {
                var fixture = await _teamRepository.SearchTeam(searchParam);
                if (fixture != null)
                {
                    response.Status = nameof(ResponseCode.Success);
                    response.StatusCode = (int)ResponseCode.Success;
                    response.Message = "Search completed succesfully";
                    response.ModelToReturn = fixture;
                    return Ok(response);
                }

                response.Status = nameof(ResponseCode.RecordNotFound);
                response.StatusCode = (int)ResponseCode.RecordNotFound;
                response.Message = "No match found";
                response.ModelToReturn = null;
                return Ok(response);
            }
            catch (Exception)
            {
                response.Status = nameof(ResponseCode.Error);
                response.StatusCode = (int)ResponseCode.Error;
                response.Message = "Oops!!!, Search could not be completed";
                response.ModelToReturn = null;
                return Ok(response);
            }
        }
    }
}