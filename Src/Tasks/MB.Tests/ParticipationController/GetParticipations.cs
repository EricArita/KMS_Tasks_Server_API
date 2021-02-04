using MB.Core.Application.Interfaces;
using MB.Core.Application.Models.Participation;
using MB.Core.Application.Models.Participation.GETSpecificResponses;
using MB.Core.Domain.DbEntities;
using MB.Infrastructure.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using static MB.Core.Domain.Constants.Enums;

namespace MB.Tests.ParticipationController
{
    public class GetParticipations
    {
        private readonly ITestOutputHelper _testOutputHelper;

        private readonly ApplicationDbContext dbContext;

        private readonly ILogger<Infrastructure.Services.Internal.ParticipationService> _logger;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly Func<object, long, Task> testFunc;

        public GetParticipations(ITestOutputHelper helper, ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<Infrastructure.Services.Internal.ParticipationService> logger)
        {
            _testOutputHelper = helper;
            dbContext = context;
            _userManager = userManager;
            _logger = logger;

            testFunc = async (uid, statusCode) =>
            {
                // Prepare data
                var fake_participation_to_get = new GetAllParticipationsModel()
                {
                    ProjectId = null,
                    UserId = 1,
                };
                var fakeUserClaims = new
                {
                    uid = uid,
                };
                var participationServiceMock = new Mock<IParticipationService>();
                participationServiceMock.Setup(service => service.GetAllParticipations(It.IsAny<long>(), It.IsAny<GetAllParticipationsModel>())).ReturnsAsync(new GetAllParticipatedProjects_OfUser_ResponseModel(null, null));
                var participationController = new WebApi.Controllers.v1.ParticipationController(participationServiceMock.Object, _userManager);

                // Mock up a case to bypass authen success
                var controllerContext = new ControllerContext();
                var httpContextMock = new Mock<HttpContext>();
                var claimPrinciple = new Mock<ClaimsPrincipal>();

                claimPrinciple.Setup(cp => cp.HasClaim(It.IsAny<Predicate<Claim>>())).Returns(true);
                claimPrinciple.Setup(cp => cp.Claims).Returns(new Claim[] {
                    new Claim("uid", fakeUserClaims.uid.ToString())
                });
                httpContextMock.Setup(context => context.User).Returns(claimPrinciple.Object);

                controllerContext.HttpContext = httpContextMock.Object;
                participationController.ControllerContext = controllerContext;
                // call add participation
                var get_participations_result = await participationController.GetAllParticipations(fake_participation_to_get);
                var final_result = get_participations_result as ObjectResult;

                // Given the uid, Assert final have to return status code
                Assert.True(final_result != null);
                Assert.True(final_result.StatusCode == statusCode);
            };
        }

        [Fact]
        public void EverythingIsFine()
        {
            testFunc(1, 200).Wait();
        }

        [Fact]
        public void FailedAtAuthorization_UidNotLong()
        {
            testFunc("aaa", 401).Wait();
        }

        [Fact]
        public void FailedAtAuthorization_UidIsLong()
        {
            testFunc(10, 401).Wait();
        }

        [Fact]
        public void FailedWhenRunningParticipationService()
        {
            Func<Task> test = async () =>
            {
                // Prepare data, this time the data has a wrong userid to mess with service layer
                var fake_participation_to_get = new GetAllParticipationsModel()
                {
                    ProjectId = null,
                    UserId = 10,
                };
                // this time uid is a valid one
                var fakeUserClaims = new
                {
                    uid = 1,
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

                // Create controller with mocked service
                var participationService = new Infrastructure.Services.Internal.ParticipationService(unitOfWorkMock.Object, _userManager, (ILogger<Infrastructure.Services.Internal.ParticipationService>)_logger);
                var participationController = new WebApi.Controllers.v1.ParticipationController(participationService, _userManager);

                // Mock up a case to pass the access token check when there is a uid field
                var controllerContext = new ControllerContext();
                var httpContextMock = new Mock<HttpContext>();
                var claimPrinciple = new Mock<ClaimsPrincipal>();

                // We mock that the user by passes authentication and got to service with the wrong id
                claimPrinciple.Setup(cp => cp.HasClaim(It.IsAny<Predicate<Claim>>())).Returns(true);
                claimPrinciple.Setup(cp => cp.Claims).Returns(new Claim[] {
                    new Claim("uid", fakeUserClaims.uid.ToString())
                });
                httpContextMock.Setup(context => context.User).Returns(claimPrinciple.Object);

                controllerContext.HttpContext = httpContextMock.Object;
                participationController.ControllerContext = controllerContext;
                IActionResult get_participations_result = null;
                // call delete participation
                try
                {
                    get_participations_result = await participationController.GetAllParticipations(fake_participation_to_get);
                }
                catch (Exception e)
                {
                    _testOutputHelper.WriteLine(e.ToString());
                }
                var final_result = get_participations_result as ObjectResult;

                // Given uid is incorrect but of the type int, Assert final have to return things other than 401 and 200
                Assert.True(final_result != null);
                Assert.True(final_result.StatusCode != 401 && final_result.StatusCode != 200);
            };

            test().Wait();
        }
    }
}
