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
    public class FixtureController : ControllerBase
    {
        private readonly IFixtureRepository _fixtureRepository;
        public FixtureController(IFixtureRepository fixtureRepository)
        {
            _fixtureRepository = fixtureRepository;            
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateFixtures(FixtureDto fixtureDto)
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
                    response.ModelToReturn = fixtureDto;
                    return Ok(response);
                }

                if (!await _fixtureRepository.ValidateFixture(fixtureDto.HomeTeamId, fixtureDto.AwayTeamId))
                {
                    var createdFixture = await _fixtureRepository.CreateFixture(fixtureDto);
                    if (createdFixture != null)
                    {
                        response.Status = nameof(ResponseCode.Success);
                        response.StatusCode = (int)ResponseCode.Success;
                        response.Message = "Fixture created successfully";
                        response.ModelToReturn = createdFixture;
                        return Ok(response);
                    }
                    else
                    {
                        response.Status = nameof(ResponseCode.Failed);
                        response.StatusCode = (int)ResponseCode.Failed;
                        response.Message = "Fixture could not be created";
                        response.ModelToReturn = fixtureDto;
                        return Ok(response);
                    }
                }
                else
                {
                    response.Status = nameof(ResponseCode.Error);
                    response.StatusCode = (int)ResponseCode.Error;
                    response.Message = "Fixture with same teams already exist.";
                    response.ModelToReturn = fixtureDto;
                    return Ok(response);
                }
            }
            catch (Exception)
            {
                response.Status = nameof(ResponseCode.Error);
                response.StatusCode = (int)ResponseCode.Error;
                response.Message = "Opps!!! An error occured";
                response.ModelToReturn = fixtureDto;
                return Ok(response);
            }
        }

        [HttpPut("UpdateFixtures/{id:int}", Name = "UpdateFixtures")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateFixtures([FromBody]FixtureDto fixtureToUpdateDto, int id)
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
                    response.ModelToReturn = fixtureToUpdateDto;
                    return Ok(response);
                }

                if (!await _fixtureRepository.ValidateFixture(fixtureToUpdateDto.HomeTeamId, fixtureToUpdateDto.AwayTeamId, id))
                {
                    //Update Fixture
                    var updatedFixture= await _fixtureRepository.UpdateFixture(fixtureToUpdateDto, id);
                    if (updatedFixture != null)
                    {
                        response.Status = nameof(ResponseCode.Success);
                        response.StatusCode = (int)ResponseCode.Success;
                        response.Message = "Fixture updated successfully";
                        response.ModelToReturn = updatedFixture;
                        return Ok(response);
                    }
                    else
                    {
                        response.Status = nameof(ResponseCode.Failed);
                        response.StatusCode = (int)ResponseCode.Failed;
                        response.Message = "Fixture could not be updated";
                        response.ModelToReturn = fixtureToUpdateDto;
                        return Ok(response);
                    }
                }
                else
                {
                    response.Status = nameof(ResponseCode.Failed);
                    response.StatusCode = (int)ResponseCode.Failed;
                    response.Message = "Another fixture with same team already exist.";
                    response.ModelToReturn = fixtureToUpdateDto;
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                response.Status = nameof(ResponseCode.Error);
                response.StatusCode = (int)ResponseCode.Error;
                response.Message = "Opps!!! An error occured";
                response.ModelToReturn = fixtureToUpdateDto;
                return Ok(response);
            }
        }

        [HttpDelete("DeleteFixture/{id:int}", Name = "DeleteFixture")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteFixture(int id)
        {
            var response = new BaseReturnDto();
            try
            {
                if (id == 0)
                {
                    response.Status = nameof(ResponseCode.Success);
                    response.StatusCode = (int)ResponseCode.Success;
                    response.Message = "No Fixture specified for deletion";
                    response.ModelToReturn = null;
                    return Ok(response);
                }

                //Delete Team
                if (await _fixtureRepository.DeleteFixture(id))
                {
                    response.Status = nameof(ResponseCode.Success);
                    response.StatusCode = (int)ResponseCode.Success;
                    response.Message = "Fixture deleted successfully";
                    response.ModelToReturn = null;
                    return Ok(response);
                }
                else
                {
                    response.Status = nameof(ResponseCode.Failed);
                    response.StatusCode = (int)ResponseCode.Failed;
                    response.Message = "Fixtured could not be deleted";
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
        public async Task<IActionResult> GetAllFixtures()
        {
            var response = new BaseReturnDto();
            try
            {
                var fixtures = await _fixtureRepository.GetAllFixture();
                if (fixtures.Count > 0)
                {
                    response.Status = nameof(ResponseCode.Success);
                    response.StatusCode = (int)ResponseCode.Success;
                    response.Message = "Fixtures retrived succesfully";
                    response.ModelToReturn = fixtures;
                    return Ok(response);
                }

                response.Status = nameof(ResponseCode.RecordNotFound);
                response.StatusCode = (int)ResponseCode.RecordNotFound;
                response.Message = "No Fixture available";
                response.ModelToReturn = null;
                return Ok(response);
            }
            catch (Exception)
            {
                response.Status = nameof(ResponseCode.Error);
                response.StatusCode = (int)ResponseCode.Error;
                response.Message = "Oops!!!, Fixtures could not be retrieved";
                response.ModelToReturn = null;
                return Ok(response);
            }
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetAllCompletedFixture()
        {
            var response = new BaseReturnDto();
            try
            {
                var fixtures = await _fixtureRepository.GetAllCompletedFixture();
                if (fixtures.Count > 0)
                {
                    response.Status = nameof(ResponseCode.Success);
                    response.StatusCode = (int)ResponseCode.Success;
                    response.Message = "Completed Fixtures retrived succesfully";
                    response.ModelToReturn = fixtures;
                    return Ok(response);
                }

                response.Status = nameof(ResponseCode.RecordNotFound);
                response.StatusCode = (int)ResponseCode.RecordNotFound;
                response.Message = "No Completed Fixture available";
                response.ModelToReturn = null;
                return Ok(response);
            }
            catch (Exception)
            {
                response.Status = nameof(ResponseCode.Error);
                response.StatusCode = (int)ResponseCode.Error;
                response.Message = "Oops!!!, Completed Fixtures could not be retrieved";
                response.ModelToReturn = null;
                return Ok(response);
            }
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetAllPendingFixture()
        {
            var response = new BaseReturnDto();
            try
            {
                var fixtures = await _fixtureRepository.GetAllPendingFixture();
                if (fixtures.Count > 0)
                {
                    response.Status = nameof(ResponseCode.Success);
                    response.StatusCode = (int)ResponseCode.Success;
                    response.Message = "Pending Fixtures retrived succesfully";
                    response.ModelToReturn = fixtures;
                    return Ok(response);
                }

                response.Status = nameof(ResponseCode.RecordNotFound);
                response.StatusCode = (int)ResponseCode.RecordNotFound;
                response.Message = "No Pending Fixture available";
                response.ModelToReturn = null;
                return Ok(response);
            }
            catch (Exception)
            {
                response.Status = nameof(ResponseCode.Error);
                response.StatusCode = (int)ResponseCode.Error;
                response.Message = "Oops!!!, Pending Fixtures could not be retrieved";
                response.ModelToReturn = null;
                return Ok(response);
            }
        }

        [HttpGet("GetFixtureById/{id:int}", Name = "GetFixtureById")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetFixtureById(int id)
        {
            var response = new BaseReturnDto();
            try
            {
                var fixture = await _fixtureRepository.GetFixtureById(id);
                if (fixture != null)
                {
                    response.Status = nameof(ResponseCode.Success);
                    response.StatusCode = (int)ResponseCode.Success;
                    response.Message = "Fixture retrived succesfully";
                    response.ModelToReturn = fixture;
                    return Ok(response);
                }

                response.Status = nameof(ResponseCode.RecordNotFound);
                response.StatusCode = (int)ResponseCode.RecordNotFound;
                response.Message = "No such Fixture is available";
                response.ModelToReturn = null;
                return Ok(response);
            }
            catch (Exception)
            {
                response.Status = nameof(ResponseCode.Error);
                response.StatusCode = (int)ResponseCode.Error;
                response.Message = "Oops!!!, Fixture could not be retrieved";
                response.ModelToReturn = null;
                return Ok(response);
            }
        }

        [HttpGet("GetFixtureByURL/{url}", Name = "GetFixtureByURL")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetFixtureByURL(string url)
        {
            var response = new BaseReturnDto();
            try
            {
                var fixture = await _fixtureRepository.GetFixtureByURL(url);
                if (fixture != null)
                {
                    response.Status = nameof(ResponseCode.Success);
                    response.StatusCode = (int)ResponseCode.Success;
                    response.Message = "Fixture retrived succesfully";
                    response.ModelToReturn = fixture;
                    return Ok(response);
                }

                response.Status = nameof(ResponseCode.RecordNotFound);
                response.StatusCode = (int)ResponseCode.RecordNotFound;
                response.Message = "No such Fixture is available";
                response.ModelToReturn = null;
                return Ok(response);
            }
            catch (Exception)
            {
                response.Status = nameof(ResponseCode.Error);
                response.StatusCode = (int)ResponseCode.Error;
                response.Message = "Oops!!!, Fixture could not be retrieved";
                response.ModelToReturn = null;
                return Ok(response);
            }
        }

        [HttpPost("SearchFixture", Name = "SearchFixture")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchFixture(string searchParam)
        {
            var response = new BaseReturnDto();
            try
            {
                var fixture = await _fixtureRepository.SearchFixture(searchParam);
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