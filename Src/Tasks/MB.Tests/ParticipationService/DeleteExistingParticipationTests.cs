using MB.Core.Application.Interfaces;
using MB.Core.Application.Models.Participation;
using MB.Core.Domain.DbEntities;
using MB.Infrastructure.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using static MB.Core.Domain.Constants.Enums;

namespace MB.Tests.ParticipationService
{
    [Collection("ParticipationService")]
    public class DeleteExistingParticipationTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        private readonly ILogger<Infrastructure.Services.Internal.ParticipationService> _logger;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ApplicationDbContext dbContext;

        // function to run data through for add participation tests
        private readonly Func<long, long?, long?, ProjectRoles?, bool, Task> innerFunc;

        public DeleteExistingParticipationTests(ITestOutputHelper helper, UserManager<ApplicationUser> userManager, ApplicationDbContext context, ILogger<Infrastructure.Services.Internal.ParticipationService> logger)
        {
            _testOutputHelper = helper;
            _logger = logger;
            _userManager = userManager;
            dbContext = context;
            innerFunc = async (queriedBy, projectId, userId, RoleId, isResultNull) =>
            {
                // Prepare data  
                DeleteParticipationModel delete_model = new DeleteParticipationModel
                {
                    RemoveFromProjectId = projectId,
                    RemoveProjectRoleId = RoleId,
                    RemoveUserId = userId
                };

                //Prepare mocks
                var userProjectRepo = new Mock<IGenericRepository<UserProjects>>();
                var projectRepo = new Mock<IGenericRepository<Project>>();
                var unitOfWorkMock = new Mock<IUnitOfWork>();
                var transaction = await dbContext.Database.BeginTransactionAsync();

                //Set up repos
                userProjectRepo.Setup(repo => repo.GetDbset()).Returns(dbContext.UserProjects);
                projectRepo.Setup(repo => repo.GetDbset()).Returns(dbContext.Project);
                // Set up unit of work with repos
                unitOfWorkMock.Setup(u => u.Repository<UserProjects>()).Returns(userProjectRepo.Object);
                unitOfWorkMock.Setup(u => u.Repository<Project>()).Returns(projectRepo.Object);
                unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
                unitOfWorkMock.Setup(u => u.CreateTransaction()).ReturnsAsync(transaction);
                
                // Run test
                // Run the add
                object result = null;
                try
                {
                    var participationService = new Infrastructure.Services.Internal.ParticipationService(unitOfWorkMock.Object, _userManager, _logger);
                    result = await participationService.DeleteExistingParticipation(queriedBy, delete_model);
                }
                catch (Exception e)
                {
                    _testOutputHelper.WriteLine(e.ToString());
                };

                // assert: we expect that there is result with current DB state
                Assert.True(isResultNull ? result == null : result != null, "Failed to run delete existing participation correctly");
            };
        }

        [Theory]
        [InlineData(1, 1, 2, ProjectRoles.PM)]
        public void DeleteParticipation_RemoveRole_AllParamsValid(long queriedBy, long? projectId, long? userId, ProjectRoles? RoleId)
        {
            innerFunc(queriedBy, projectId, userId, RoleId, false).Wait();
        }

        [Theory]
        [InlineData(1, 1, 1, ProjectRoles.PM)]
        public void DeleteParticipation_RemoveRoles_RoleOfUserInProjectNOTExist(long queriedBy, long? projectId, long? userId, ProjectRoles? RoleId)
        {
            innerFunc(queriedBy, projectId, userId, RoleId, true).Wait();
        }

        [Theory]
        [InlineData(1, 1, 2, null)]
        public void DeleteParticipation_RemoveUserFromProject_AllParamsValid(long queriedBy, long? projectId, long? userId, ProjectRoles? RoleId)
        {
            innerFunc(queriedBy, projectId, userId, RoleId, false).Wait();
        }

        [Theory]
        [InlineData(1, 1, 1, null)]
        public void DeleteParticipation_RemoveUserFromProject_UserIsOwner(long queriedBy, long? projectId, long? userId, ProjectRoles? RoleId)
        {
            innerFunc(queriedBy, projectId, userId, RoleId, true).Wait();
        }

        [Theory]
        [InlineData(1, 1, 10, null)]
        public void DeleteParticipation_UserIdInvalid(long queriedBy, long? projectId, long? userId, ProjectRoles? RoleId)
        {
            innerFunc(queriedBy, projectId, userId, RoleId, true).Wait();
        }

        [Theory]
        [InlineData(1, 100, 1, null)]
        public void DeleteParticipation_ProjectIdInvalid(long queriedBy, long? projectId, long? userId, ProjectRoles? RoleId)
        {
            innerFunc(queriedBy, projectId, userId, RoleId, true).Wait();
        }
    }
}
