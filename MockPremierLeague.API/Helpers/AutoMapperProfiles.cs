using AutoMapper;
using MockPremierLeague.API.Dtos;
using MockPremierLeague.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MockPremierLeague.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            //Users
            CreateMap<UserForRegistrationDto, User>();
            CreateMap<UserForAdminRegistrationDto, User>();

            //Teams
            CreateMap<TeamDto ,Team>();

            //Fixtures
            CreateMap<FixtureDto, Fixture>();
        }
    }
}
