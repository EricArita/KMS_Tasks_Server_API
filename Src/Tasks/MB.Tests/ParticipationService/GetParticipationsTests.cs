using MB.Core.Application.Interfaces;
using MB.Core.Application.Models.Participation;
using MB.Core.Application.Models.Participation.GETSpecificResponses;
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

        private readonly IParticipationService _participationService;

        public GetParticipationsTests(ITestOutputHelper helper, IParticipationService participationService)
        {
            _testOutputHelper = helper;
            _participationService = participationService;
        }

        [Theory]
        [InlineData(1, 1, 1)]
        public void GetRolesOfHimself_HeDidParticipateInProject(long queriedBy, long? projectId, long? userId)
        {
            Func<Task> innerFunc = async () =>
            {
                // Prepare data
                GetAllParticipationsModel model = new GetAllParticipationsModel
                {
                    ProjectId = projectId,
                    UserId = userId
                };

                // Run test
                IGetAllParticipations_ResponseModel result = null;
                try
                {
                    result = await _participationService.GetAllParticipations(queriedBy, model);
                }
                catch (Exception e)
                {
                    _testOutputHelper.WriteLine(e.Message);
                }

                // assert: we expect that there is result with current DB state
                Assert.False(result == null, "Failed to run get all participations correctly");
            };

            innerFunc().Wait();
        }

        [Theory]
        [InlineData(1, 9, 1)]
        public void GetRolesOfHimself_HeDidNotParticipateInProject(long queriedBy, long? projectId, long? userId)
        {
            Func<Task> innerFunc = async () =>
            {
                // Prepare data
                GetAllParticipationsModel model = new GetAllParticipationsModel
                {
                    ProjectId = projectId,
                    UserId = userId
                };

                // Run test
                IGetAllParticipations_ResponseModel result = null;
                try
                {
                    result = await _participationService.GetAllParticipations(queriedBy, model);
                }
                catch (Exception e)
                {
                    _testOutputHelper.WriteLine(e.Message);
                }

                // assert: we expect that there is no result with current DB state
                Assert.True(result == null, "Failed to run get all participations correctly");
            };

            innerFunc().Wait();
        }

        [Theory]
        [InlineData(1, 10, 1)]
        public void GetRolesOfUser_ProjectIdInvalid(long queriedBy, long? projectId, long? userId)
        {
            Func<Task> innerFunc = async () =>
            {
                // Prepare data
                GetAllParticipationsModel model = new GetAllParticipationsModel
                {
                    ProjectId = projectId,
                    UserId = userId
                };

                // Run test
                IGetAllParticipations_ResponseModel result = null;
                try
                {
                    result = await _participationService.GetAllParticipations(queriedBy, model);
                }
                catch (Exception e)
                {
                    _testOutputHelper.WriteLine(e.Message);
                }

                // assert: we expect that there is result with current DB state
                Assert.True(result == null, "Failed to run get all participations correctly");
            };

            innerFunc().Wait();
        }

        [Theory]
        [InlineData(1, 1, 3)]
        public void GetRolesOfUser_UserIdInvalid(long queriedBy, long? projectId, long? userId)
        {
            Func<Task> innerFunc = async () =>
            {
                // Prepare data
                GetAllParticipationsModel model = new GetAllParticipationsModel
                {
                    ProjectId = projectId,
                    UserId = userId
                };

                // Run test
                IGetAllParticipations_ResponseModel result = null;
                try
                {
                    result = await _participationService.GetAllParticipations(queriedBy, model);
                }
                catch (Exception e)
                {
                    _testOutputHelper.WriteLine(e.Message);
                }

                // assert: we expect that there is result with current DB state
                Assert.True(result == null, "Failed to run get all participations correctly");
            };

            innerFunc().Wait();
        }

        [Theory]
        [InlineData(1, 1, 2)]
        public void GetRolesOfOthers_OthersParticipateInProject(long queriedBy, long? projectId, long? userId)
        {
            Func<Task> innerFunc = async () =>
            {
                // Prepare data
                GetAllParticipationsModel model = new GetAllParticipationsModel
                {
                    ProjectId = projectId,
                    UserId = userId
                };

                // Run test
                IGetAllParticipations_ResponseModel result = null;
                try
                {
                    result = await _participationService.GetAllParticipations(queriedBy, model);
                }
                catch (Exception e)
                {
                    _testOutputHelper.WriteLine(e.Message);
                }

                // result
                Assert.True(result != null, "There should be roles of other user in DB because other did participate in project 1. Results doesn't indicate that");
            };

            innerFunc().Wait();
        }

        [Theory]
        [InlineData(1, 2, 2)]
        public void GetRolesOfOthers_OthersNotParticipateInProject(long queriedBy, long? projectId, long? userId)
        {
            Func<Task> innerFunc = async () =>
            {
                // Prepare data
                GetAllParticipationsModel model = new GetAllParticipationsModel
                {
                    ProjectId = projectId,
                    UserId = userId
                };

                // Run test
                IGetAllParticipations_ResponseModel result = null;
                try
                {
                    result = await _participationService.GetAllParticipations(queriedBy, model);
                }
                catch (Exception e)
                {
                    _testOutputHelper.WriteLine(e.Message);
                }

                // result
                Assert.True(result == null, "There should be no roles of other user in DB because other did not participate in project 1");
            };

            innerFunc().Wait();
        }

        [Theory]
        [InlineData(1, null, null)]
        public void NothingProvided(long queriedBy, long? projectId, long? userId)
        {
            Func<Task> innerFunc = async () =>
            {
                // Prepare data
                GetAllParticipationsModel model = new GetAllParticipationsModel
                {
                    ProjectId = projectId,
                    UserId = userId
                };

                // Run test
                IGetAllParticipations_ResponseModel result = null;
                try
                {
                    result = await _participationService.GetAllParticipations(queriedBy, model);
                }
                catch (Exception e)
                {
                    _testOutputHelper.WriteLine(e.Message);
                }

                // result
                Assert.True(result == null, "There should be no result at all because user did not type in anything");
            };

            innerFunc().Wait();
        }

        [Theory]
        [InlineData(1, 1, null)]
        public void GetUsersInProject_UserDidParticipateInProject(long queriedBy, long? projectId, long? userId)
        {
            Func<Task> innerFunc = async () =>
            {
                // Prepare data
                GetAllParticipationsModel model = new GetAllParticipationsModel
                {
                    ProjectId = projectId,
                    UserId = userId
                };

                // Run test
                IGetAllParticipations_ResponseModel result = null;
                try
                {
                    result = await _participationService.GetAllParticipations(queriedBy, model);
                }
                catch (Exception e)
                {
                    _testOutputHelper.WriteLine(e.Message);
                }

                // result
                Assert.True(result != null, "There should be a users list of this project 1 since querying user 1 did participate in project 1");
            };

            innerFunc().Wait();
        }

        [Theory]
        [InlineData(1, 9, null)]
        public void GetUsersInProject_UserDidNotParticipateInProject(long queriedBy, long? projectId, long? userId)
        {
            Func<Task> innerFunc = async () =>
            {
                // Prepare data
                GetAllParticipationsModel model = new GetAllParticipationsModel
                {
                    ProjectId = projectId,
                    UserId = userId
                };

                // Run test
                IGetAllParticipations_ResponseModel result = null;
                try
                {
                    result = await _participationService.GetAllParticipations(queriedBy, model);
                }
                catch (Exception e)
                {
                    _testOutputHelper.WriteLine(e.Message);
                }

                // result
                Assert.True(result == null, "There should not be a users list of this project 9 since querying user 1 did participate in project 9");
            };

            innerFunc().Wait();
        }

        [Theory]
        [InlineData(1, 10, null)]
        public void GetUsersInProject_ProjectIdInvalid(long queriedBy, long? projectId, long? userId)
        {
            Func<Task> innerFunc = async () =>
            {
                // Prepare data
                GetAllParticipationsModel model = new GetAllParticipationsModel
                {
                    ProjectId = projectId,
                    UserId = userId
                };

                // Run test
                IGetAllParticipations_ResponseModel result = null;
                try
                {
                    result = await _participationService.GetAllParticipations(queriedBy, model);
                }
                catch (Exception e)
                {
                    _testOutputHelper.WriteLine(e.Message);
                }

                // result
                Assert.True(result == null, "There should not be a users list of this project 10 since there is no project 10");
            };

            innerFunc().Wait();
        }

        [Theory]
        [InlineData(1, null, 1)]
        public void GetProjectsOfUser_UserIsHimself(long queriedBy, long? projectId, long? userId)
        {
            Func<Task> innerFunc = async () =>
            {
                // Prepare data
                GetAllParticipationsModel model = new GetAllParticipationsModel
                {
                    ProjectId = projectId,
                    UserId = userId
                };

                // Run test
                IGetAllParticipations_ResponseModel result = null;
                try
                {
                    result = await _participationService.GetAllParticipations(queriedBy, model);
                }
                catch (Exception e)
                {
                    _testOutputHelper.WriteLine(e.Message);
                }

                // result
                Assert.True(result != null, "There should be a projects list along with roles of user 1 since querying user 1 did participate in some projects");
            };

            innerFunc().Wait();
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

                // Run test
                GetAllParticipatedProjects_OfUser_ResponseModel result = null;
                try
                {
                    result = (GetAllParticipatedProjects_OfUser_ResponseModel)await _participationService.GetAllParticipations(queriedBy, model);
                }
                catch (Exception e)
                {
                    _testOutputHelper.WriteLine(e.Message);
                }

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
            Func<Task> innerFunc = async () =>
            {
                // Prepare data
                GetAllParticipationsModel model = new GetAllParticipationsModel
                {
                    ProjectId = projectId,
                    UserId = userId
                };

                // Run test
                GetAllParticipatedProjects_OfUser_ResponseModel result = null;
                try
                {
                    result = (GetAllParticipatedProjects_OfUser_ResponseModel)await _participationService.GetAllParticipations(queriedBy, model);
                }
                catch (Exception e)
                {
                    _testOutputHelper.WriteLine(e.Message);
                }

                // result
                Assert.True(result == null,
                    "There should not be any record since user id is invalid");
            };

            innerFunc().Wait();
        }
    }
}
