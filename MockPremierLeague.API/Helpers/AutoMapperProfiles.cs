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
            //Teams
            CreateMap<TeamDto ,Team>();
        }
    }
}
