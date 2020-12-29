using Core.Application.Helper;
using Core.Application.Helper.Exceptions.Project;
using Core.Application.Interfaces;
using Core.Application.Models;
using Core.Application.Models.Project;
using Core.Domain.DbEntities;
using Infrastructure.Persistence.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Controllers.v1.Utils;

namespace WebApi.Controllers.v1
{
    [Area("project-management")]
    public class ProjectController : BaseController
    {
        private IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
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
                return Ok(new HttpResponse<ProjectResponseModel>(true, addedProject, message: "Successfully added project"));
            }
            catch (Exception ex)
            {
                if (ex is ProjectServiceException exception)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("A problem occurred when processing the content of your request, please recheck your request params: ");
                    sb.AppendLine(exception.Message);
                    uint? statusCode = ServiceExceptionsProcessor.getStatusCode(exception.Message);
                    if(statusCode != null && statusCode.HasValue)
                    {
                        return StatusCode((int) statusCode.Value, new HttpResponse<object>(false, null, sb.ToString()));
                    }              
                }
                return StatusCode(500, new HttpResponse<Exception>(false, ex, "Server encountered an exception"));
            }
        }

        [HttpGet("projects")]
        public async Task<IActionResult> GetAllProjects()
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
                };
                // Carry on with the business logic
                IEnumerable<ProjectResponseModel> projectParticipations = await _projectService.GetAllProjects(serviceModel);
                return Ok(new HttpResponse<IEnumerable<ProjectResponseModel>>(true, projectParticipations, message: "Successfully fetched projects of user"));
            }
            catch (Exception ex)
            {
                if (ex is ProjectServiceException exception)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("A problem occurred when processing the content of your request, please recheck your request params: ");
                    sb.AppendLine(exception.Message);
                    uint? statusCode = ServiceExceptionsProcessor.getStatusCode(exception.Message);
                    if (statusCode != null && statusCode.HasValue)
                    {
                        return StatusCode((int)statusCode.Value, new HttpResponse<object>(false, null, sb.ToString()));
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
                    uint? statusCode = ServiceExceptionsProcessor.getStatusCode(exception.Message);
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
                return Ok(new HttpResponse<ProjectResponseModel>(true, participatedProject, message: "Successfully patched specified project of user"));
            }
            catch (Exception ex)
            {
                if (ex is ProjectServiceException exception)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("A problem occurred when processing the content of your request, please recheck your request params: ");
                    sb.AppendLine(exception.Message);
                    uint? statusCode = ServiceExceptionsProcessor.getStatusCode(exception.Message);
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
                    uint? statusCode = ServiceExceptionsProcessor.getStatusCode(exception.Message);
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
