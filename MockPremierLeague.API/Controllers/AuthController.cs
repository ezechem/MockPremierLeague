using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MockPremierLeague.API.Data;
using MockPremierLeague.API.Dtos;
using MockPremierLeague.API.Enumerators;
using MockPremierLeague.API.Models;
using Newtonsoft.Json;

namespace MockPremierLeague.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleInManager;

        public AuthController(AppDbContext appDbContext, IConfiguration config, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleInManager)
        {
            _appDbContext = appDbContext;
            _config = config;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleInManager = roleInManager;
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var user = await _userManager.FindByNameAsync(userForLoginDto.Username);
            var result = await _signInManager.CheckPasswordSignInAsync(user, userForLoginDto.Password, false);

            if (result.Succeeded)
            {
                var appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == userForLoginDto.Username.ToUpper());
                var userRoles = await _userManager.GetRolesAsync(user);

                //Check if user is active or not
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetRoles()
        {
            var response = new BaseReturnDto();
            try
            {
                var roles = await _roleInManager.Roles.ToListAsync();
                if (roles != null)
                {
                    response.Status = nameof(ResponseCode.Success);
                    response.StatusCode = (int)ResponseCode.Success;
                    response.Message = "Roles Retrived Successfully";
                    response.ModelToReturn = roles;
                    return Ok(response);
                }

                response.Status = nameof(ResponseCode.RecordNotFound);
                response.StatusCode = (int)ResponseCode.RecordNotFound;
                response.Message = "No roles available";
                response.ModelToReturn = roles;
                return Ok(response);

            }
            catch (Exception)
            {
                response.Status = nameof(ResponseCode.Error);
                response.StatusCode = (int)ResponseCode.Error;
                response.Message = "Roles could not be retrieved ";
                response.ModelToReturn = null;
                return Ok(response);
            }
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAdminUser(UserForAdminRegistrationDto userForAdminRegistrationDto)
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
                    response.ModelToReturn = userForAdminRegistrationDto;
                    return Ok(response);
                }

                List<string> roles = new List<string>();
                foreach (var role in userForAdminRegistrationDto.Roles)
                {
                    roles.Add(role);
                }

                //Role Check
                if (roles.Count == 0)
                {
                    response.Status = nameof(ResponseCode.Error);
                    response.StatusCode = (int)ResponseCode.Error;
                    response.Message = "User need to belong to at least a role.";
                    response.ModelToReturn = userForAdminRegistrationDto;
                    return Ok(response);
                }

                var newUser = _mapper.Map<User>(userForAdminRegistrationDto);

                //Create User
                IdentityResult result = await _userManager.CreateAsync(newUser, userForAdminRegistrationDto.Password);
                if (result.Succeeded)
                {
                    var user = _userManager.FindByNameAsync(newUser.UserName).Result;

                    _userManager.AddToRolesAsync(user, roles).Wait();

                    response.Status = nameof(ResponseCode.Success);
                    response.StatusCode = (int)ResponseCode.Success;
                    response.Message = "User created succesfully";
                    response.ModelToReturn = userForAdminRegistrationDto;
                    return Ok(response);
                }

                //Admin User Creation not successful
                var identityerrorList = (from item in result.Errors
                                         select item.Description).ToList();
                string identityerrorMessage = JsonConvert.SerializeObject(identityerrorList);
                response.Status = nameof(ResponseCode.Error);
                response.StatusCode = (int)ResponseCode.Error;
                response.Message = identityerrorMessage;
                response.ModelToReturn = userForAdminRegistrationDto;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Status = nameof(ResponseCode.Error);
                response.StatusCode = (int)ResponseCode.Error;
                response.Message = "An error occured during user creation";
                response.ModelToReturn = userForAdminRegistrationDto;
                return Ok(response);
            }
        }

        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserForRegistrationDto userForRegistrationDto)
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

                var newUser = _mapper.Map<User>(userForRegistrationDto);
                //Create User
                IdentityResult result = await _userManager.CreateAsync(newUser, userForRegistrationDto.Password);
                if (result.Succeeded)
                {
                    var user = _userManager.FindByNameAsync(newUser.UserName).Result;

                    _userManager.AddToRolesAsync(user, new[] { "User" }).Wait();

                    response.Status = nameof(ResponseCode.Success);
                    response.StatusCode = (int)ResponseCode.Success;
                    response.Message = "User created succesfully";
                    response.ModelToReturn = userForRegistrationDto;
                    return Ok(response);
                }

                //User Creation not successful
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