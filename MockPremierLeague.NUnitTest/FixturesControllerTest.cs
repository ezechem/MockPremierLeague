using Microsoft.AspNetCore.Mvc;
using MockPremierLeague.API.Contracts;
using MockPremierLeague.API.Controllers;
using MockPremierLeague.API.Dtos;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MockPremierLeague.NUnitTest
{
    public class FixturesControllerTest
    {
        private readonly Mock<IFixtureRepository> _fixtureRepositoryMock = new Mock<IFixtureRepository>();

        private readonly FixtureController _fixtureController;
        public FixturesControllerTest()
        {
            _fixtureController = new FixtureController(_fixtureRepositoryMock.Object);
        }

        [Test]
        public async Task Get_ShouldGetAllFixtures()
        {
            // Arrange
            // Act
            var result = await _fixtureController.GetAllFixtures();

            // Assert
            Assert.NotNull(result);

            var createdResult = result as OkObjectResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual(200, createdResult.StatusCode);
        }

        [Test]
        public async Task Post_ShouldInsertTeam()
        {
            // Arrange
            var newFixture= new FixtureDto
            {
                HomeTeamId = 1,
                AwayTeamId = 2,
                MatchCode = "Fixture1",
                Staduim = "Stamford Bridge",
                MatchDate =new DateTime(2019,06,01),
                MatchTime = new TimeSpan(16,0,0),
                Status = false
            };

            // Act
            var result = await _fixtureController.CreateFixtures(newFixture);

            // Assert
            Assert.NotNull(result);

            var createdResult = result as OkObjectResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual(200, createdResult.StatusCode);
        }

        [Test]
        public async Task Put_ShouldUpdateTeam()
        {
            // Arrange
            var newFixture = new FixtureDto
            {
                HomeTeamId = 1,
                AwayTeamId = 2,
                MatchCode = "Fixture1",
                Staduim = "Stamford Bridge",
                MatchDate = new DateTime(2019, 06, 01),
                MatchTime = new TimeSpan(16, 0, 0),
                Status = false
            };

            // Act
            var result = await _fixtureController.UpdateFixtures(newFixture, 1);

            // Assert
            Assert.NotNull(result);

            var createdResult = result as OkObjectResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual(200, createdResult.StatusCode);
        }

        [Test]
        public async Task Delete_ShouldDeleteTeam()
        {
            // Arrange

            // Act
            var result = await _fixtureController.DeleteFixture(1);

            // Assert
            Assert.NotNull(result);

            var createdResult = result as OkObjectResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual(200, createdResult.StatusCode);
        }
    }
}
