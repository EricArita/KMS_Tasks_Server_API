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

namespace WebApi.Controllers.v1
{
    public class ProjectController : BaseController
    {
        private IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost("project")]
        public async Task<IActionResult> AddNewProject(NewProjectModel newProject)
        {
            try
            {
                // Check validity of the request
                if (newProject.CreatedBy != null) {
                    return Unauthorized(new Response<object>(false, null, "Unauthorized creation of project"));
                }
                var claimsManager = HttpContext.User;
                if(!claimsManager.HasClaim(c => c.Type == "uid"))
                {
                    return Unauthorized(new Response<object>(false, null, "Token provided is invalid because there is no valid confidential claim"));
                }
                // Extract uid from token
                int uid;
                try
                {
                    uid = int.Parse(claimsManager.Claims.FirstOrDefault(c => c.Type == "uid").Value);
                } catch (Exception)
                {
                    return Unauthorized(new Response<object>(false, null, "Token provided is invalid because the value for the claim is invalid"));
                }
                newProject.CreatedBy = uid;

                // Carry on with the business logic
                ProjectResponseModel addedProject = await _projectService.AddNewProject(newProject);
                return Ok(new Response<ProjectResponseModel>(true, addedProject, message: "Successfully added project"));
            }
            catch (Exception ex)
            {
                if (ex is ProjectServiceException exception)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("A problem occurred when processing the content of your request, please recheck your request params: ");
                    sb.AppendLine(exception.Message);
                    return StatusCode(exception.StatusCode, new Response<object>(false, null, sb.ToString()));
                }
                return StatusCode(500, new Response<Exception>(false, ex, "Server encountered an exception"));
            }
        }

        [HttpGet("projects")]
        public async Task<IActionResult> GetAllProjects()
        {
            try
            {
                //Check validity of the token
                var claimsManager = HttpContext.User;
                if (!claimsManager.HasClaim(c => c.Type == "uid"))
                {
                    return Unauthorized(new Response<object>(false, null, "Token provided is invalid because there is no valid confidential claim"));
                }
                // Extract uid from token
                int uid;
                try 
                {
                    uid = int.Parse(claimsManager.Claims.FirstOrDefault(c => c.Type == "uid").Value);
                } catch (Exception)
                {
                    return Unauthorized(new Response<object>(false, null, "Token provided is invalid because the value for the confidential claim is invalid"));
                }

                // If passes all tests, then we submit it to the service layer
                GetAllProjectsModel model = new GetAllProjectsModel()
                {
                    UserID = uid,
                };
                // Carry on with the business logic
                IEnumerable<ProjectResponseModel> projectParticipations = await _projectService.GetAllProjects(model);
                return Ok(new Response<IEnumerable<ProjectResponseModel>>(true, projectParticipations, message: "Successfully fetched projects of user"));
            }
            catch (Exception ex)
            {
                if (ex is ProjectServiceException exception)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("A problem occurred when processing the content of your request, please recheck your request params: ");
                    sb.AppendLine(exception.Message);
                    return StatusCode(exception.StatusCode, new Response<object>(false, null, sb.ToString()));
                }
                return StatusCode(500, new Response<Exception>(false, ex, "Server encountered an exception"));
            }
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetAParticularProject(int projectId)
        {
            try
            {
                //Check validity of the token
                var claimsManager = HttpContext.User;
                if (!claimsManager.HasClaim(c => c.Type == "uid"))
                {
                    return Unauthorized(new Response<object>(false, null, "Token provided is invalid because there is no valid confidential claim"));
                }
                // Extract uid from token
                int uid;
                try
                {
                    uid = int.Parse(claimsManager.Claims.FirstOrDefault(c => c.Type == "uid").Value);
                }
                catch (Exception)
                {
                    return Unauthorized(new Response<object>(false, null, "Token provided is invalid because the value for the confidential claim is invalid"));
                }

                // If passes all tests, then we submit it to the service layer
                GetOneProjectModel model = new GetOneProjectModel()
                {
                    ProjectId = projectId,
                    UserId = uid,
                };
                // Carry on with the business logic
                ProjectResponseModel participatedProject = await _projectService.GetOneProject(model);
                return Ok(new Response<ProjectResponseModel>(true, participatedProject, message: "Successfully fetched specified project of user"));
            }
            catch (Exception ex)
            {
                if (ex is ProjectServiceException exception)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("A problem occurred when processing the content of your request, please recheck your request params: ");
                    sb.AppendLine(exception.Message);
                    return StatusCode(exception.StatusCode, new Response<object>(false, null, sb.ToString()));
                }
                return StatusCode(500, new Response<Exception>(false, ex, "Server encountered an exception"));
            }
        }

        [HttpPatch("project/{projectId}")]
        public async Task<IActionResult> UpdateExistingProject(int projectId, UpdateProjectInfoModel model)
        {
            try
            {
                //Check validity of the token
                if (model.CreatedBy != null)
                {
                    return Unauthorized(new Response<object>(false, null, "Unauthorized creation of project"));
                }
                var claimsManager = HttpContext.User;
                if (!claimsManager.HasClaim(c => c.Type == "uid"))
                {
                    return Unauthorized(new Response<object>(false, null, "Token provided is invalid because there is no valid confidential claim"));
                }
                // Extract uid from token
                int uid;
                try
                {
                    uid = int.Parse(claimsManager.Claims.FirstOrDefault(c => c.Type == "uid").Value);
                }
                catch (Exception)
                {
                    return Unauthorized(new Response<object>(false, null, "Token provided is invalid because the value for the confidential claim is invalid"));
                }

                // If passes all tests, then we submit it to the service layer
                model.CreatedBy = uid;
                // Carry on with the business logic
                ProjectResponseModel participatedProject = await _projectService.UpdateProjectInfo(projectId, model);
                return Ok(new Response<ProjectResponseModel>(true, participatedProject, message: "Successfully patched specified project of user"));
            }
            catch (Exception ex)
            {
                if (ex is ProjectServiceException exception)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("A problem occurred when processing the content of your request, please recheck your request params: ");
                    sb.AppendLine(exception.Message);
                    return StatusCode(exception.StatusCode, new Response<object>(false, null, sb.ToString()));
                }
                return StatusCode(500, new Response<Exception>(false, ex, "Server encountered an exception"));
            }
        }

        [HttpDelete("project/{projectId}")]
        public async Task<IActionResult> DeleteExistingProject(int projectId)
        {
            try
            {
                //Check validity of the token
                var claimsManager = HttpContext.User;
                if (!claimsManager.HasClaim(c => c.Type == "uid"))
                {
                    return Unauthorized(new Response<object>(false, null, "Token provided is invalid because there is no valid confidential claim"));
                }
                // Extract uid from token
                int uid;
                try
                {
                    uid = int.Parse(claimsManager.Claims.FirstOrDefault(c => c.Type == "uid").Value);
                }
                catch (Exception)
                {
                    return Unauthorized(new Response<object>(false, null, "Token provided is invalid because the value for the confidential claim is invalid"));
                }

                // If passes all tests, then we submit it to the service layer
                // Carry on with the business logic
                ProjectResponseModel participatedProject = await _projectService.SoftDeleteExistingProject(projectId, uid);
                return Ok(new Response<ProjectResponseModel>(true, participatedProject, message: "Successfully patched specified project of user"));
            }
            catch (Exception ex)
            {
                if (ex is ProjectServiceException exception)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("A problem occurred when processing the content of your request, please recheck your request params: ");
                    sb.AppendLine(exception.Message);
                    return StatusCode(exception.StatusCode, new Response<object>(false, null, sb.ToString()));
                }
                return StatusCode(500, new Response<Exception>(false, ex, "Server encountered an exception"));
            }
        }
    }
}
