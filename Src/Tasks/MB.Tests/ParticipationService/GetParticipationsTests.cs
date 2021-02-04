using MB.Core.Application.Interfaces;
using MB.Core.Application.Models.Participation;
using MB.Core.Application.Models.Participation.GETSpecificResponses;
using MB.Core.Domain.DbEntities;
using MB.Infrastructure.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace MB.Tests.ParticipationService
{
    [Collection("ParticipationService")]
    public class GetParticipationsTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        private readonly ILogger _logger; 

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ApplicationDbContext dbContext;

        // function to run data through for add participation tests
        private readonly Func<long, long?, long?, bool, Task> innerFunc;

        public GetParticipationsTests(ITestOutputHelper helper, UserManager<ApplicationUser> userManager, ApplicationDbContext context, ILogger<Infrastructure.Services.Internal.ParticipationService> logger)
        {
            _testOutputHelper = helper;
            _userManager = userManager;
            _logger = logger;
            dbContext = context;

            innerFunc = async (queriedBy, projectId, userId, isResultNull) =>
            {
                // Prepare data
                GetAllParticipationsModel model = new GetAllParticipationsModel
                {
                    ProjectId = projectId,
                    UserId = userId
                };

                //Prepare mocks
                var userProjectRepo = new Mock<IGenericRepository<UserProjects>>();
                var projectRepo = new Mock<IGenericRepository<Project>>();
                var projectRolesRepo = new Mock<IGenericRepository<ProjectRole>>();
                var unitOfWorkMock = new Mock<IUnitOfWork>();
                var transaction = await dbContext.Database.BeginTransactionAsync();

                //Set up repos
                userProjectRepo.Setup(repo => repo.GetDbset()).Returns(dbContext.UserProjects);
                projectRepo.Setup(repo => repo.GetDbset()).Returns(dbContext.Project);
                projectRolesRepo.Setup(repo => repo.GetDbset()).Returns(dbContext.ProjectRoles);
                // Set up unit of work with repos
                unitOfWorkMock.Setup(u => u.Repository<UserProjects>()).Returns(userProjectRepo.Object);
                unitOfWorkMock.Setup(u => u.Repository<Project>()).Returns(projectRepo.Object);
                unitOfWorkMock.Setup(u => u.Repository<ProjectRole>()).Returns(projectRolesRepo.Object);
                unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
                unitOfWorkMock.Setup(u => u.CreateTransaction()).ReturnsAsync(transaction);

                // Run test
                // Run the add
                IGetAllParticipations_ResponseModel result = null;
                try
                {
                    var participationService = new Infrastructure.Services.Internal.ParticipationService(unitOfWorkMock.Object, _userManager, (ILogger<Infrastructure.Services.Internal.ParticipationService>)_logger);
                    result = await participationService.GetAllParticipations(queriedBy, model);
                }
                catch (Exception e)
                {
                    _testOutputHelper.WriteLine(e.ToString());
                };

                // assert: we expect that there is result with current DB state
                Assert.True(isResultNull ? result == null : result != null, "Failed to run get participations correctly");
                return;
            };
        }

        [Theory]
        [InlineData(1, 1, 1)]
        public void GetRolesOfHimself_HeDidParticipateInProject(long queriedBy, long? projectId, long? userId)
        {
            innerFunc(queriedBy, projectId, userId, false).Wait();
        }

        [Theory]
        [InlineData(1, 9, 1)]
        public void GetRolesOfHimself_HeDidNotParticipateInProject(long queriedBy, long? projectId, long? userId)
        {
            innerFunc(queriedBy, projectId, userId, true).Wait();
        }

        [Theory]
        [InlineData(1, 10, 1)]
        public void GetRolesOfUser_ProjectIdInvalid(long queriedBy, long? projectId, long? userId)
        {
            innerFunc(queriedBy, projectId, userId, true).Wait();
        }

        [Theory]
        [InlineData(1, 1, 3)]
        public void GetRolesOfUser_UserIdInvalid(long queriedBy, long? projectId, long? userId)
        {
            innerFunc(queriedBy, projectId, userId, true).Wait();
        }

        [Theory]
        [InlineData(1, 1, 2)]
        public void GetRolesOfOthers_OthersParticipateInProject(long queriedBy, long? projectId, long? userId)
        {
            innerFunc(queriedBy, projectId, userId, false).Wait();
        }

        [Theory]
        [InlineData(1, 2, 2)]
        public void GetRolesOfOthers_OthersNotParticipateInProject(long queriedBy, long? projectId, long? userId)
        {
            innerFunc(queriedBy, projectId, userId, true).Wait();
        }

        [Theory]
        [InlineData(1, null, null)]
        public void NothingProvided(long queriedBy, long? projectId, long? userId)
        {
            innerFunc(queriedBy, projectId, userId, true).Wait();
        }

        [Theory]
        [InlineData(1, 1, null)]
        public void GetUsersInProject_UserDidParticipateInProject(long queriedBy, long? projectId, long? userId)
        {
            innerFunc(queriedBy, projectId, userId, false).Wait();
        }

        [Theory]
        [InlineData(1, 9, null)]
        public void GetUsersInProject_UserDidNotParticipateInProject(long queriedBy, long? projectId, long? userId)
        {
            innerFunc(queriedBy, projectId, userId, true).Wait();
        }

        [Theory]
        [InlineData(1, 10, null)]
        public void GetUsersInProject_ProjectIdInvalid(long queriedBy, long? projectId, long? userId)
        {
            innerFunc(queriedBy, projectId, userId, true).Wait();
        }

        [Theory]
        [InlineData(1, null, 1)]
        public void GetProjectsOfUser_UserIsHimself(long queriedBy, long? projectId, long? userId)
        {
            innerFunc(queriedBy, projectId, userId, false).Wait();
        }

        [Theory]
        [InlineData(1, null, 2)]
        public void GetProjectsOfUser_UserIsOthers(long queriedBy, long? projectId, long? userId)
        {
            Func<Task> innerFunc = async () =>
            {
                // Prepare data
                GetAllParticipationsModel model = new GetAllParticipationsModel
                {
                    ProjectId = projectId,
                    UserId = userId
                };

                //Prepare mocks
                var userProjectRepo = new Mock<IGenericRepository<UserProjects>>();
                var projectRepo = new Mock<IGenericRepository<Project>>();
                var projectRolesRepo = new Mock<IGenericRepository<ProjectRole>>();
                var unitOfWorkMock = new Mock<IUnitOfWork>();
                var transaction = await dbContext.Database.BeginTransactionAsync();

                //Set up repos
                userProjectRepo.Setup(repo => repo.GetDbset()).Returns(dbContext.UserProjects);
                projectRepo.Setup(repo => repo.GetDbset()).Returns(dbContext.Project);
                projectRolesRepo.Setup(repo => repo.GetDbset()).Returns(dbContext.ProjectRoles);
                // Set up unit of work with repos
                unitOfWorkMock.Setup(u => u.Repository<UserProjects>()).Returns(userProjectRepo.Object);
                unitOfWorkMock.Setup(u => u.Repository<Project>()).Returns(projectRepo.Object);
                unitOfWorkMock.Setup(u => u.Repository<ProjectRole>()).Returns(projectRolesRepo.Object);
                unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
                unitOfWorkMock.Setup(u => u.CreateTransaction()).ReturnsAsync(transaction);

                // Run test
                // Run the add
                GetAllParticipatedProjects_OfUser_ResponseModel result = null;
                try
                {
                    var participationService = new Infrastructure.Services.Internal.ParticipationService(unitOfWorkMock.Object, _userManager, (ILogger<Infrastructure.Services.Internal.ParticipationService>)_logger);
                    result = (GetAllParticipatedProjects_OfUser_ResponseModel)await participationService.GetAllParticipations(queriedBy, model);
                }
                catch (Exception e)
                {
                    _testOutputHelper.WriteLine(e.ToString());
                };

                // result: SHOULD only have one record since, between user 1 and user 2 there is only ONE common project
                Assert.True(result != null && result.ParticipatedProjects != null && result.ParticipatedProjects.Count == 1,
                    "There should be a projects list along with roles of user 2, length should be 1 since querying user 1 only participated in 1 project with user 2, the other project of user 2 should not be in this list");
            };

            innerFunc().Wait();
        }

        [Theory]
        [InlineData(1, null, 3)]
        public void GetProjectsOfUser_UserIdInvalid(long queriedBy, long? projectId, long? userId)
        {
            innerFunc(queriedBy, projectId, userId, true).Wait();
        }
    }
}
