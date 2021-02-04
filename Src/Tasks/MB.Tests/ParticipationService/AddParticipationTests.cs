using MB.Core.Application.Helper.Exceptions.Participation;
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
    public class AddParticipationTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        private readonly ILogger _logger;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ApplicationDbContext dbContext;

        // function to run data through for add participation tests
        private readonly Func<long, long?, long?, ProjectRoles?, bool, Task> innerFunc;

        public AddParticipationTests(ITestOutputHelper helper, UserManager<ApplicationUser> userManager, ApplicationDbContext context, ILogger<Infrastructure.Services.Internal.ParticipationService> logger)
        {
            _testOutputHelper = helper;
            _userManager = userManager;
            _logger = logger;
            dbContext = context;

            innerFunc = async (queriedBy, projectId, userId, RoleId, isResultNull) => {
                // Prepare data  
                NewParticipationModel create_model = new NewParticipationModel
                {
                    ProjectId = projectId,
                    UserId = userId,
                    RoleId = RoleId
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
                unitOfWorkMock.Setup(u => u.Entry(It.IsAny<UserProjects>())).Returns<EntityEntry<UserProjects>>(null);

                // Run test
                // Run the add
                ParticipationResponseModel result = null;
                try
                {
                    var participationService = new Infrastructure.Services.Internal.ParticipationService(unitOfWorkMock.Object, _userManager, (ILogger<Infrastructure.Services.Internal.ParticipationService>)_logger);
                    result = await participationService.AddNewParticipation(queriedBy, create_model);
                }
                catch (Exception e)
                {
                    _testOutputHelper.WriteLine(e.ToString());
                };

                // assert: we expect that there is result with current DB state
                Assert.True(isResultNull ? result == null : result != null, "Failed to run create new participation correctly");
                return;
            };
        }

        [Theory]
        [InlineData(1, 1, 2, ProjectRoles.Leader)]
        public void AddParticipation_AllParamsValid(long queriedBy, long? projectId, long? userId, ProjectRoles? RoleId)
        {
            innerFunc(queriedBy, projectId, userId, RoleId, false).Wait();
        }

        [Theory]
        [InlineData(1, 1, 2, ProjectRoles.PM)]
        public void AddParticipation_DuplicatedRole_OfUser_InProject(long queriedBy, long? projectId, long? userId, ProjectRoles? RoleId)
        {
            innerFunc(queriedBy, projectId, userId, RoleId, true).Wait();
        }

        [Theory]
        [InlineData(1, 1, 10, ProjectRoles.PM)]
        public void AddParticipation_InvalidUserId(long queriedBy, long? projectId, long? userId, ProjectRoles? RoleId)
        {
            innerFunc(queriedBy, projectId, userId, RoleId, true).Wait();
        }

        [Theory]
        [InlineData(1, 10, 2, ProjectRoles.PM)]
        public void AddParticipation_InvalidProjectId(long queriedBy, long? projectId, long? userId, ProjectRoles? RoleId)
        {
            innerFunc(queriedBy, projectId, userId, RoleId, true).Wait();
        }

        [Theory]
        [InlineData(1, 9, 2, ProjectRoles.PM)]
        public void AddParticipation_CreatorHaveNoRights_ToAddParticipation(long queriedBy, long? projectId, long? userId, ProjectRoles? RoleId)
        {
            innerFunc(queriedBy, projectId, userId, RoleId, true).Wait();
        }

        [Theory]
        [InlineData(1, 1, 2, ProjectRoles.Owner)]
        public void AddParticipation_AddSecondOwner(long queriedBy, long? projectId, long? userId, ProjectRoles? RoleId)
        {
            innerFunc(queriedBy, projectId, userId, RoleId, true).Wait();
        }
    }
}
