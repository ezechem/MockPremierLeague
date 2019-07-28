using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockPremierLeague.API.Contracts;
using MockPremierLeague.API.Controllers;
using MockPremierLeague.API.Data;
using MockPremierLeague.API.Models;
using MockPremierLeague.API.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MockPremierLeague.Test
{
    public class TeamControllerUnitTest
    {       
        [Fact]
        public async Task TestGetTeamsAsync()
        {
            //// Arrange
            //var dbContext = DbContextMocker.GetMockPremierLeagueImportersDbContext(nameof(TestGetTeamsAsync));
            //var teamRepository = new TeamRepository(dbContext, _mapper);
            //var _teamController = new TeamController(teamRepository);

            //// Act
            //var result = await _teamController.GetAllTeams();
            //dbContext.Dispose();

            //// Assert
            //Assert.NotNull(result);

            //var createdResult = result as OkObjectResult;
            //Assert.NotNull(createdResult);
            //Assert.Equal(200, createdResult.StatusCode);

            // Arrange
            var testObject = new Team
            {
                Name = "Arsenal",
                Code = "ARS",
                Address = "Arsenal",
                CoachName = "Unai Emery",
                YearFounded = 1886,
                Stadium = "Emirates Stadium"
            };
            var testList = new List<Team>() { testObject };
            var mapper = new Mock<IMapper>();


            var dbSetMock = new Mock<DbSet<Team>>();
            dbSetMock.As<IQueryable<Team>>().Setup(x => x.Provider).Returns
                                                 (testList.AsQueryable().Provider);
            dbSetMock.As<IQueryable<Team>>().Setup(x => x.Expression).
                                                 Returns(testList.AsQueryable().Expression);
            dbSetMock.As<IQueryable<Team>>().Setup(x => x.ElementType).Returns
                                                 (testList.AsQueryable().ElementType);
            dbSetMock.As<IQueryable<Team>>().Setup(x => x.GetEnumerator()).Returns
                                                 (testList.AsQueryable().GetEnumerator());

            var context = new Mock<AppDbContext>();
            context.Setup(x => x.Set<Team>()).Returns(dbSetMock.Object);

            // Act
            var repository = new TeamRepository(context.Object, mapper.Object);
            var result = repository.GetAllTeams();



            // Assert
            // Assert
            Assert.Equal(testList, result.Result);

        }
    }
}
