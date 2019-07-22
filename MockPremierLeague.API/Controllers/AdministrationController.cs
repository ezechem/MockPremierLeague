using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MockPremierLeague.API.Contracts;
using MockPremierLeague.API.Data;
using MockPremierLeague.API.Dtos;
using MockPremierLeague.API.Enumerators;
using MockPremierLeague.API.Models;
using Newtonsoft.Json;

namespace MockPremierLeague.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministrationController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly IAdminRepository _adminRepository;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleInManager;

        public AdministrationController(AppDbContext appDbContext, IAdminRepository adminRepository, IConfiguration config, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleInManager)
        {
            _appDbContext = appDbContext;
            _adminRepository = adminRepository;
            _config = config;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleInManager = roleInManager;
        }

        //AdAppDbContext _appDbContext;ministration Authentication
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var user = await _userManager.FindByNameAsync(userForLoginDto.Username);
            var result = await _signInManager.CheckPasswordSignInAsync(user, userForLoginDto.Password, false);

            if (result.Succeeded)
            {
                var appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == userForLoginDto.Username.ToUpper());
                var userRoles = await _userManager.GetRolesAsync(user);

                //User is not an Admin
                if (!userRoles.Contains("Admin"))
                {
                    return Unauthorized();
                }

                //Check if admin user is active or not
                if (!appUser.IsActive)
                {
                    return Unauthorized();
                }

                return Ok(new
                {
                    token = GenerateJwtToken(appUser).Result,
                    user = appUser
                });
            }
            return Unauthorized();
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllTeam()
        {
            var response = new BaseReturnDto();
            try
            {
                var teams= await _adminRepository.GetAllTeams();
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

        [HttpGet("GetAllTeamById/{id:int}", Name = "GetAllTeamById")]
        [Route("[action]")]
        public async Task<IActionResult> GetAllTeamById(int id)
        {
            var response = new BaseReturnDto();
            try
            {
                var teams = await _adminRepository.GetTeamById(id);
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

        [HttpPost]
        [Route("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAdminUser(UserForRegistrationDto userForRegistrationDto)
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
                    response.ModelToReturn = userForRegistrationDto;
                    return Ok(response);
                }


                List<string> roles = new List<string>();
                foreach (var role in userForRegistrationDto.Roles)
                {
                    roles.Add(role);
                }

                //Role Check
                if (roles.Count == 0)
                {
                    response.Status = nameof(ResponseCode.Error);
                    response.StatusCode = (int)ResponseCode.Error;
                    response.Message = "User need to belong to at least a role.";
                    response.ModelToReturn = userForRegistrationDto;
                    return Ok(response);
                }

                var newUser = _mapper.Map<User>(userForRegistrationDto);
                //Create User
                IdentityResult result = await _userManager.CreateAsync(newUser, userForRegistrationDto.Password);
                if (result.Succeeded)
                {
                    var user = _userManager.FindByNameAsync(newUser.UserName).Result;

                    _userManager.AddToRolesAsync(user, roles).Wait();

                    response.Status = nameof(ResponseCode.Success);
                    response.StatusCode = (int)ResponseCode.Success;
                    response.Message = "User created succesfully";
                    response.ModelToReturn = userForRegistrationDto;
                    return Ok(response);
                }


                //Admin User Creation not successful
                var identityerrorList = (from item in result.Errors
                                         select item.Description).ToList();
                string identityerrorMessage = JsonConvert.SerializeObject(identityerrorList);
                response.Status = nameof(ResponseCode.Error);
                response.StatusCode = (int)ResponseCode.Error;
                response.Message = identityerrorMessage;
                response.ModelToReturn = userForRegistrationDto;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Status = nameof(ResponseCode.Error);
                response.StatusCode = (int)ResponseCode.Error;
                response.Message = "An error occured during user creation";
                response.ModelToReturn = userForRegistrationDto;
                return Ok(response);
            }
        }

        //Team
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

                if (!await _adminRepository.ValidateTeam(teamDto.Name))
                {
                    var createdTeam = await _adminRepository.CreateTeam(teamDto);
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

                if (!await _adminRepository.ValidateTeam(teamToUpdateDto.Name, id))
                {
                    //Update Team
                    var updatedTeam = await _adminRepository.UpdateTeam(teamToUpdateDto, id);
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
                if (await _adminRepository.DeleteTeam(id))
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


        #region "Local Methods"
        private async Task<string> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        #endregion
    }
}
