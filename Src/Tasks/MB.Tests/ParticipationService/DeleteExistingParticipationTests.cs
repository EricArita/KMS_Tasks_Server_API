using MB.Core.Application.Interfaces;
using MB.Core.Application.Models.Participation;
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

        private readonly IParticipationService _participationService;

        public DeleteExistingParticipationTests(ITestOutputHelper helper, IParticipationService participationService)
        {
            _testOutputHelper = helper;
            _participationService = participationService;
        }

        [Theory]
        [InlineData(1, 1, 2, ProjectRoles.PM)]
        public void DeleteParticipation_RemoveRole_AllParamsValid(long queriedBy, long? projectId, long? userId, ProjectRoles? RoleId)
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
                // Run the remove
                object result = null;
                try
                {
                    result = await _participationService.DeleteExistingParticipation(queriedBy, delete_model);
                }
                catch (Exception e)
                {
                    _testOutputHelper.WriteLine(e.Message);
                }

                // add again if removed successful to keep the previous state of DB
                if (result != null)
                {
                    try
                    {
                        await _participationService.AddNewParticipation(queriedBy, create_model);
                    }
                    catch (Exception e)
                    {
                        _testOutputHelper.WriteLine(e.Message);
                    }
                }

                // assert: we expect that there is result with current DB state
                Assert.True(result != null, "Failed to run delete existing participation correctly");
            };

            innerFunc().Wait();
        }

        [Theory]
        [InlineData(1, 1, 1, ProjectRoles.PM)]
        public void DeleteParticipation_RemoveRoles_RoleOfUserInProjectNOTExist(long queriedBy, long? projectId, long? userId, ProjectRoles? RoleId)
        {
            Func<Task> innerFunc = async () =>
            {
                // Prepare data  
                NewParticipationModel create_model = new NewParticipationModel
                {
                    ProjectId = projectId,
                    UserId = userId,
                    RoleId = ProjectRoles.PM
                };
                DeleteParticipationModel delete_model = new DeleteParticipationModel
                {
                    RemoveFromProjectId = projectId,
                    RemoveProjectRoleId = RoleId,
                    RemoveUserId = userId
                };

                // Run test
                // Run the remove
                object result = null;
                try
                {
                    result = await _participationService.DeleteExistingParticipation(queriedBy, delete_model);
                }
                catch (Exception e)
                {
                    _testOutputHelper.WriteLine(e.Message);
                }

                // add again if removed successful to keep the previous state of DB
                if (result != null)
                {
                    try
                    {
                        await _participationService.AddNewParticipation(queriedBy, create_model);
                    }
                    catch (Exception e)
                    {
                        _testOutputHelper.WriteLine(e.Message);
                    }
                }

                // assert: we expect that there is result with current DB state
                Assert.True(result == null, "Failed to run delete existing participation correctly");
            };

            innerFunc().Wait();
        }

        [Theory]
        [InlineData(1, 1, 2, null)]
        public void DeleteParticipation_RemoveUserFromProject_AllParamsValid(long queriedBy, long? projectId, long? userId, ProjectRoles? RoleId)
        {
            Func<Task> innerFunc = async () =>
            {
                // Prepare data  
                NewParticipationModel create_model = new NewParticipationModel
                {
                    ProjectId = projectId,
                    UserId = userId,
                    RoleId = ProjectRoles.PM
                };
                DeleteParticipationModel delete_model = new DeleteParticipationModel
                {
                    RemoveFromProjectId = projectId,
                    RemoveProjectRoleId = RoleId,
                    RemoveUserId = userId
                };

                // Run test
                // Run the remove
                object result = null;
                try
                {
                    result = await _participationService.DeleteExistingParticipation(queriedBy, delete_model);
                }
                catch (Exception e)
                {
                    _testOutputHelper.WriteLine(e.Message);
                }

                // add again if removed successful to keep the previous state of DB
                if (result != null)
                {
                    try
                    {
                        await _participationService.AddNewParticipation(queriedBy, create_model);
                    }
                    catch (Exception e)
                    {
                        _testOutputHelper.WriteLine(e.Message);
                    }
                }

                // assert: we expect that there is result with current DB state
                Assert.True(result != null, "Failed to run delete existing participation correctly");
            };

            innerFunc().Wait();
        }

        [Theory]
        [InlineData(1, 1, 1, null)]
        public void DeleteParticipation_RemoveUserFromProject_UserIsOwner(long queriedBy, long? projectId, long? userId, ProjectRoles? RoleId)
        {
            Func<Task> innerFunc = async () =>
            {
                // Prepare data  
                NewParticipationModel create_model = new NewParticipationModel
                {
                    ProjectId = projectId,
                    UserId = userId,
                    RoleId = ProjectRoles.PM
                };
                DeleteParticipationModel delete_model = new DeleteParticipationModel
                {
                    RemoveFromProjectId = projectId,
                    RemoveProjectRoleId = RoleId,
                    RemoveUserId = userId
                };

                // Run test
                // Run the remove
                object result = null;
                try
                {
                    result = await _participationService.DeleteExistingParticipation(queriedBy, delete_model);
                }
                catch (Exception e)
                {
                    _testOutputHelper.WriteLine(e.Message);
                }

                // add again if removed successful to keep the previous state of DB
                if (result != null)
                {
                    try
                    {
                        await _participationService.AddNewParticipation(queriedBy, create_model);
                    }
                    catch (Exception e)
                    {
                        _testOutputHelper.WriteLine(e.Message);
                    }
                }

                // assert: we expect that there is result with current DB state
                Assert.True(result == null, "Failed to run delete existing participation correctly");
            };

            innerFunc().Wait();
        }

        [Theory]
        [InlineData(1, 1, 10, null)]
        public void DeleteParticipation_UserIdInvalid(long queriedBy, long? projectId, long? userId, ProjectRoles? RoleId)
        {
            Func<Task> innerFunc = async () =>
            {
                // Prepare data  
                NewParticipationModel create_model = new NewParticipationModel
                {
                    ProjectId = projectId,
                    UserId = userId,
                    RoleId = ProjectRoles.PM
                };
                DeleteParticipationModel delete_model = new DeleteParticipationModel
                {
                    RemoveFromProjectId = projectId,
                    RemoveProjectRoleId = RoleId,
                    RemoveUserId = userId
                };

                // Run test
                // Run the remove
                object result = null;
                try
                {
                    result = await _participationService.DeleteExistingParticipation(queriedBy, delete_model);
                }
                catch (Exception e)
                {
                    _testOutputHelper.WriteLine(e.Message);
                }

                // add again if removed successful to keep the previous state of DB
                if (result != null)
                {
                    try
                    {
                        await _participationService.AddNewParticipation(queriedBy, create_model);
                    }
                    catch (Exception e)
                    {
                        _testOutputHelper.WriteLine(e.Message);
                    }
                }

                // assert: we expect that there is result with current DB state
                Assert.True(result == null, "Failed to run delete existing participation correctly");
            };

            innerFunc().Wait();
        }

        [Theory]
        [InlineData(1, 100, 1, null)]
        public void DeleteParticipation_ProjectIdInvalid(long queriedBy, long? projectId, long? userId, ProjectRoles? RoleId)
        {
            Func<Task> innerFunc = async () =>
            {
                // Prepare data  
                NewParticipationModel create_model = new NewParticipationModel
                {
                    ProjectId = projectId,
                    UserId = userId,
                    RoleId = ProjectRoles.PM
                };
                DeleteParticipationModel delete_model = new DeleteParticipationModel
                {
                    RemoveFromProjectId = projectId,
                    RemoveProjectRoleId = RoleId,
                    RemoveUserId = userId
                };

                // Run test
                // Run the remove
                object result = null;
                try
                {
                    result = await _participationService.DeleteExistingParticipation(queriedBy, delete_model);
                }
                catch (Exception e)
                {
                    _testOutputHelper.WriteLine(e.Message);
                }

                // add again if removed successful to keep the previous state of DB
                if (result != null)
                {
                    try
                    {
                        await _participationService.AddNewParticipation(queriedBy, create_model);
                    }
                    catch (Exception e)
                    {
                        _testOutputHelper.WriteLine(e.Message);
                    }
                }

                // assert: we expect that there is result with current DB state
                Assert.True(result == null, "Failed to run delete existing participation correctly");
            };

            innerFunc().Wait();
        }
    }
}
