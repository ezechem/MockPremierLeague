using Microsoft.AspNetCore.Mvc;
using MockPremierLeague.API.Contracts;
using MockPremierLeague.API.Controllers;
using MockPremierLeague.API.Dtos;
using MockPremierLeague.API.Models;
using Moq;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

namespace MockPremierLeague.NUnitTest
{
   public class TeamControllerTest
    {
        private readonly Mock<ITeamRepository> _teamRepositoryMock = new Mock<ITeamRepository>();

        private readonly TeamController _teamController;
        public TeamControllerTest()
        {
            _teamController = new TeamController(_teamRepositoryMock.Object);
        }

        [Test]
        public async Task Get_ShouldGetAllTeams()
        {
            // Arrange
            // Act
            var result = await _teamController.GetAllTeams();

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
            var newTeam = new TeamDto
            {
                Name = "Arsenal",
                Code = "ARS",
                Address = "Arsenal",
                CoachName = "Unai Emery",
                YearFounded = 1886,
                Stadium = "Emirates Stadium"
            };

            // Act
            var result = await _teamController.CreateTeam(newTeam);

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
            var newTeam = new TeamDto
            {
                Name = "Arsenal",
                Code = "ARS",
                Address = "Arsenal",
                CoachName = "Unai Emery",
                YearFounded = 1886,
                Stadium = "Emirates Stadium"
            };

            // Act
            var result = await _teamController.UpdateTeam(newTeam,1);

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
            var result = await _teamController.DeleteTeam(1);

            // Assert
            Assert.NotNull(result);

            var createdResult = result as OkObjectResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual(200, createdResult.StatusCode);
        }
    }
}
