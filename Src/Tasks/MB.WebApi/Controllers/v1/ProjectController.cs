﻿using MB.Core.Application.Helper;
using MB.Core.Application.Helper.Exceptions.Project;
using MB.Core.Application.Interfaces;
using MB.Core.Application.Models;
using MB.Core.Application.Models.Project;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MB.WebApi.Controllers.v1.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using MB.Core.Domain.DbEntities;
using MB.WebApi.Hubs.v1;
using Microsoft.AspNetCore.SignalR;
using MB.Core.Application.Interfaces.Misc;

namespace MB.WebApi.Controllers.v1
{
    [Area("project-management")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProjectController : BaseController
    {
        private readonly IProjectService _projectService;
        private readonly IHubContext<GlobalHub> _hubContext;

        public ProjectController(IProjectService projectService, UserManager<ApplicationUser> userManager, IHubContext<GlobalHub> hubContext) : base(userManager)
        {
            _projectService = projectService;
            _hubContext = hubContext;
        }

        [HttpPost("project")]
        public async Task<IActionResult> AddNewProject([FromBody] NewProjectModel newProject)
        {
            try
            {
                // Check validity of the request
                var claimsManager = HttpContext.User;
                long? uid = null;
                try
                {
                    uid = GetUserId(claimsManager);
                }
                catch (Exception e)
                {
                    return Unauthorized(e.Message);
                }

                if (!uid.HasValue)
                {
                    return Unauthorized("Unauthorized individuals cannot access this route");
                }

                // Carry on with the business logic
                ProjectResponseModel addedProject = await _projectService.AddNewProject(uid.Value, newProject);

                GetAllProjectsModel fetchAllProjects = new GetAllProjectsModel()
                {
                    UserID = uid.Value
                };
                var resulting = await _projectService.GetAllProjects(fetchAllProjects);
                await _hubContext.Clients.Group($"User{uid.Value}Group").SendAsync("projects-list-changed", new { projects =  resulting.Projects});

                return Ok(new HttpResponse<ProjectResponseModel>(true, addedProject, message: "Successfully added project"));
            }
            catch (Exception ex)
            {
                if (ex is ProjectServiceException exception)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("A problem occurred when processing the content of your request, please recheck your request params: ");
                    sb.AppendLine(exception.Message);
                    uint? statusCode = ServiceExceptionsProcessor.GetStatusCode(exception.Message);
                    if(statusCode != null && statusCode.HasValue)
                    {
                        return StatusCode((int) statusCode.Value, new HttpResponse<object>(false, null, sb.ToString()));
                    }              
                }
                return StatusCode(500, new HttpResponse<Exception>(false, ex, "Server encountered an exception"));
            }
        }

        [HttpGet("projects")]
        public async Task<IActionResult> GetAllProjects([FromQuery] GetAllProjectsRequestModel model)
        {
            try
            {
                //Check validity of the token
                var claimsManager = HttpContext.User;
                long? uid = null;
                try
                {
                    uid = GetUserId(claimsManager);
                }
                catch (Exception e)
                {
                    return Unauthorized(e.Message);
                }

                if (!uid.HasValue)
                {
                    return Unauthorized("Unauthorized individuals cannot access this route");
                }

                // If passes all tests, then we submit it to the service layer
                GetAllProjectsModel serviceModel = new GetAllProjectsModel()
                {
                    UserID = uid.Value,
                    ProjectName = model.ProjectName,
                    ItemPerPage = model.ItemPerPage,
                    PageNumber = model.PageNumber
                };
                // Carry on with the business logic
                GetAllProjectsResponseModel projectParticipations = await _projectService.GetAllProjects(serviceModel);
                return Ok(new HttpResponse<GetAllProjectsResponseModel>(true, projectParticipations, message: "Successfully fetched projects of user"));
            }
            catch (Exception ex)
            {
                if (ex is ProjectServiceException exception)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("A problem occurred when processing the content of your request, please recheck your request params: ");
                    sb.AppendLine(exception.Message);
                    uint? statusCode = ServiceExceptionsProcessor.GetStatusCode(exception.Message);
                    if (statusCode != null && statusCode.HasValue)
                    {
                        return StatusCode((int)statusCode.Value, new HttpResponse<Dictionary<string, object>>(false, exception.ExtraData, sb.ToString()));
                    }
                }
                return StatusCode(500, new HttpResponse<Exception>(false, ex, "Server encountered an exception"));
            }
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetAParticularProject(long projectId)
        {
            try
            {
                //Check validity of the token
                var claimsManager = HttpContext.User;
                long? uid = null;
                try
                {
                    uid = GetUserId(claimsManager);
                }
                catch (Exception e)
                {
                    return Unauthorized(e.Message);
                }

                if (!uid.HasValue)
                {
                    return Unauthorized("Unauthorized individuals cannot access this route");
                }

                // If passes all tests, then we submit it to the service layer
                GetOneProjectModel model = new GetOneProjectModel()
                {
                    ProjectId = projectId,
                    UserId = uid.Value,
                };
                // Carry on with the business logic
                ProjectResponseModel participatedProject = await _projectService.GetOneProject(model);
                return Ok(new HttpResponse<ProjectResponseModel>(true, participatedProject, message: "Successfully fetched specified project of user"));
            }
            catch (Exception ex)
            {
                if (ex is ProjectServiceException exception)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("A problem occurred when processing the content of your request, please recheck your request params: ");
                    sb.AppendLine(exception.Message);
                    uint? statusCode = ServiceExceptionsProcessor.GetStatusCode(exception.Message);
                    if (statusCode != null && statusCode.HasValue)
                    {
                        return StatusCode((int)statusCode.Value, new HttpResponse<object>(false, null, sb.ToString()));
                    }
                }
                return StatusCode(500, new HttpResponse<Exception>(false, ex, "Server encountered an exception"));
            }
        }

        [HttpPatch("project/{projectId}")]
        public async Task<IActionResult> UpdateExistingProject(long projectId, [FromBody] UpdateProjectInfoModel model)
        {
            try
            {
                //Check validity of the token
                var claimsManager = HttpContext.User;
                long? uid = null;
                try
                {
                    uid = GetUserId(claimsManager);
                }
                catch (Exception e)
                {
                    return Unauthorized(e.Message);
                }

                if (!uid.HasValue)
                {
                    return Unauthorized("Unauthorized individuals cannot access this route");
                }

                // If passes all tests, then we submit it to the service layer
                // Carry on with the business logic
                ProjectResponseModel participatedProject = await _projectService.UpdateProjectInfo(projectId, uid.Value, model);

                GetAllProjectsModel fetchAllProjects = new GetAllProjectsModel()
                {
                    UserID = uid.Value
                };
                var resulting = await _projectService.GetAllProjects(fetchAllProjects);
                await _hubContext.Clients.Group($"User{uid.Value}Group").SendAsync("projects-list-changed", new { projects = resulting.Projects });

                return Ok(new HttpResponse<ProjectResponseModel>(true, participatedProject, message: "Successfully patched specified project of user"));
            }
            catch (Exception ex)
            {
                if (ex is ProjectServiceException exception)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("A problem occurred when processing the content of your request, please recheck your request params: ");
                    sb.AppendLine(exception.Message);
                    uint? statusCode = ServiceExceptionsProcessor.GetStatusCode(exception.Message);
                    if (statusCode != null && statusCode.HasValue)
                    {
                        return StatusCode((int)statusCode.Value, new HttpResponse<object>(false, null, sb.ToString()));
                    }
                }
                return StatusCode(500, new HttpResponse<Exception>(false, ex, "Server encountered an exception"));
            }
        }

        [HttpDelete("project/{projectId}")]
        public async Task<IActionResult> DeleteExistingProject(long projectId)
        {
            try
            {
                //Check validity of the token
                var claimsManager = HttpContext.User;
                long? uid = null;
                try
                {
                    uid = GetUserId(claimsManager);
                }
                catch (Exception e)
                {
                    return Unauthorized(e.Message);
                }

                if (!uid.HasValue)
                {
                    return Unauthorized("Unauthorized individuals cannot access this route");
                }

                // If passes all tests, then we submit it to the service layer
                // Carry on with the business logic
                ProjectResponseModel participatedProject = await _projectService.SoftDeleteExistingProject(projectId, uid.Value);
                return Ok(new HttpResponse<ProjectResponseModel>(true, participatedProject, message: "Successfully deleted specified project of user"));
            }
            catch (Exception ex)
            {
                if (ex is ProjectServiceException exception)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("A problem occurred when processing the content of your request, please recheck your request params: ");
                    sb.AppendLine(exception.Message);
                    uint? statusCode = ServiceExceptionsProcessor.GetStatusCode(exception.Message);
                    if (statusCode != null && statusCode.HasValue)
                    {
                        return StatusCode((int)statusCode.Value, new HttpResponse<object>(false, null, sb.ToString()));
                    }
                }
                return StatusCode(500, new HttpResponse<Exception>(false, ex, "Server encountered an exception"));
            }
        }
    }
}
