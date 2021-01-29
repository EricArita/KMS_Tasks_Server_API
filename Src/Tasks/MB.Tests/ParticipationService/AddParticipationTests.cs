using MB.Core.Application.Interfaces;
using MB.Core.Application.Models.Participation;
using MB.Core.Domain.Constants;
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

        private readonly IParticipationService _participationService;

        public AddParticipationTests(ITestOutputHelper helper, IParticipationService participationService)
        {
            _testOutputHelper = helper;
            _participationService = participationService;
        }

        [Theory]
        [InlineData(1, 1, 2, ProjectRoles.Leader)]
        public void AddParticipation_AllParamsValid(long queriedBy, long? projectId, long? userId, ProjectRoles? RoleId)
        {
            Func<Task> innerFunc = async () => {
                // Prepare data  
                NewParticipationModel create_model = new NewParticipationModel
                {
                    ProjectId = projectId,
                    UserId = userId,
                    RoleId = RoleId
                };
                DeleteParticipationModel delete_model = new DeleteParticipationModel
                {
                    RemoveFromProjectId = projectId,
                    RemoveProjectRoleId = RoleId,
                    RemoveUserId = userId
                };

                // Run test
                // Run the add
                ParticipationResponseModel result = null;
                try
                {
                    result = await _participationService.AddNewParticipation(queriedBy, create_model);
                }
                catch (Exception e)
                {
                    _testOutputHelper.WriteLine(e.Message);
                }
                // Remove the add if added successful to keep the previous state of DB
                if (result != null)
                {
                    try
                    {
                        await _participationService.DeleteExistingParticipation(queriedBy, delete_model);
                    }
                    catch (Exception e)
                    {
                        _testOutputHelper.WriteLine(e.Message);
                    }
                }

                // assert: we expect that there is result with current DB state
                Assert.True(result != null, "Failed to run create new participation correctly");

                return;
            };

            innerFunc().Wait();
        }

        [Theory]
        [InlineData(1, 1, 2, ProjectRoles.PM)]
        public void AddParticipation_DuplicatedRole_OfUser_InProject(long queriedBy, long? projectId, long? userId, ProjectRoles? RoleId)
        {
            Func<Task> innerFunc = async () =>
            {
                // Prepare data  
                NewParticipationModel create_model = new NewParticipationModel
                {
                    ProjectId = projectId,
                    UserId = userId,
                    RoleId = RoleId
                };
                DeleteParticipationModel delete_model = new DeleteParticipationModel
                {
                    RemoveFromProjectId = projectId,
                    RemoveProjectRoleId = RoleId,
                    RemoveUserId = userId
                };

                // Run test
                // Run the add
                ParticipationResponseModel result = null;
                try
                {
                    result = await _participationService.AddNewParticipation(queriedBy, create_model);
                }
                catch (Exception e)
                {
                    _testOutputHelper.WriteLine(e.Message);
                }
                // Remove the add if added successful to keep the previous state of DB
                if (result != null)
                {
                    try
                    {
                        await _participationService.DeleteExistingParticipation(queriedBy, delete_model);
                    }
                    catch (Exception e)
                    {
                        _testOutputHelper.WriteLine(e.Message);
                    }
                }

                // assert: we expect that there is result with current DB state
                Assert.True(result == null, "Failed to run create new participation correctly");
            };

            innerFunc().Wait();
        }

        [Theory]
        [InlineData(1, 1, 10, ProjectRoles.PM)]
        public void AddParticipation_InvalidUserId(long queriedBy, long? projectId, long? userId, ProjectRoles? RoleId)
        {
            Func<Task> innerFunc = async () =>
            {
                // Prepare data  
                NewParticipationModel create_model = new NewParticipationModel
                {
                    ProjectId = projectId,
                    UserId = userId,
                    RoleId = RoleId
                };
                DeleteParticipationModel delete_model = new DeleteParticipationModel
                {
                    RemoveFromProjectId = projectId,
                    RemoveProjectRoleId = RoleId,
                    RemoveUserId = userId
                };

                // Run test
                // Run the add
                ParticipationResponseModel result = null;
                try
                {
                    result = await _participationService.AddNewParticipation(queriedBy, create_model);
                }
                catch (Exception e)
                {
                    _testOutputHelper.WriteLine(e.Message);
                }
                // Remove the add if added successful to keep the previous state of DB
                if (result != null)
                {
                    try
                    {
                        await _participationService.DeleteExistingParticipation(queriedBy, delete_model);
                    }
                    catch (Exception e)
                    {
                        _testOutputHelper.WriteLine(e.Message);
                    }
                }

                // assert: we expect that there is result with current DB state
                Assert.True(result == null, "Failed to run create new participation correctly");
            };

            innerFunc().Wait();
        }

        [Theory]
        [InlineData(1, 10, 2, ProjectRoles.PM)]
        public void AddParticipation_InvalidProjectId(long queriedBy, long? projectId, long? userId, ProjectRoles? RoleId)
        {
            Func<Task> innerFunc = async () =>
            {
                // Prepare data  
                NewParticipationModel create_model = new NewParticipationModel
                {
                    ProjectId = projectId,
                    UserId = userId,
                    RoleId = RoleId
                };
                DeleteParticipationModel delete_model = new DeleteParticipationModel
                {
                    RemoveFromProjectId = projectId,
                    RemoveProjectRoleId = RoleId,
                    RemoveUserId = userId
                };

                // Run test
                // Run the add
                ParticipationResponseModel result = null;
                try
                {
                    result = await _participationService.AddNewParticipation(queriedBy, create_model);
                }
                catch (Exception e)
                {
                    _testOutputHelper.WriteLine(e.Message);
                }
                // Remove the add if added successful to keep the previous state of DB
                if (result != null)
                {
                    try
                    {
                        await _participationService.DeleteExistingParticipation(queriedBy, delete_model);
                    }
                    catch (Exception e)
                    {
                        _testOutputHelper.WriteLine(e.Message);
                    }
                }

                // assert: we expect that there is result with current DB state
                Assert.True(result == null, "Failed to run create new participation correctly");
            };

            innerFunc().Wait();
        }

        [Theory]
        [InlineData(1, 9, 2, ProjectRoles.PM)]
        public void AddParticipation_CreatorHaveNoRights_ToAddParticipation(long queriedBy, long? projectId, long? userId, ProjectRoles? RoleId)
        {
            Func<Task> innerFunc = async () =>
            {
                // Prepare data  
                NewParticipationModel create_model = new NewParticipationModel
                {
                    ProjectId = projectId,
                    UserId = userId,
                    RoleId = RoleId
                };
                DeleteParticipationModel delete_model = new DeleteParticipationModel
                {
                    RemoveFromProjectId = projectId,
                    RemoveProjectRoleId = RoleId,
                    RemoveUserId = userId
                };

                // Run test
                // Run the add
                ParticipationResponseModel result = null;
                try
                {
                    result = await _participationService.AddNewParticipation(queriedBy, create_model);
                }
                catch (Exception e)
                {
                    _testOutputHelper.WriteLine(e.Message);
                }
                // Remove the add if added successful to keep the previous state of DB
                if (result != null)
                {
                    try
                    {
                        await _participationService.DeleteExistingParticipation(queriedBy, delete_model);
                    }
                    catch (Exception e)
                    {
                        _testOutputHelper.WriteLine(e.Message);
                    }
                }

                // assert: we expect that there is result with current DB state
                Assert.True(result == null, "Failed to run create new participation correctly");
            };

            innerFunc().Wait();
        }

        [Theory]
        [InlineData(1, 1, 2, ProjectRoles.Owner)]
        public void AddParticipation_AddSecondOwner(long queriedBy, long? projectId, long? userId, ProjectRoles? RoleId)
        {
            Func<Task> innerFunc = async () =>
            {
                // Prepare data  
                NewParticipationModel create_model = new NewParticipationModel
                {
                    ProjectId = projectId,
                    UserId = userId,
                    RoleId = RoleId
                };
                DeleteParticipationModel delete_model = new DeleteParticipationModel
                {
                    RemoveFromProjectId = projectId,
                    RemoveProjectRoleId = RoleId,
                    RemoveUserId = userId
                };

                // Run test
                // Run the add
                ParticipationResponseModel result = null;
                try
                {
                    result = await _participationService.AddNewParticipation(queriedBy, create_model);
                }
                catch (Exception e)
                {
                    _testOutputHelper.WriteLine(e.Message);
                }
                // Remove the add if added successful to keep the previous state of DB
                if (result != null)
                {
                    try
                    {
                        await _participationService.DeleteExistingParticipation(queriedBy, delete_model);
                    }
                    catch (Exception e)
                    {
                        _testOutputHelper.WriteLine(e.Message);
                    }
                }

                // assert: we expect that there is result with current DB state
                Assert.True(result == null, "Failed to run create new participation correctly");
            };

            innerFunc().Wait();
        }
    }
}
