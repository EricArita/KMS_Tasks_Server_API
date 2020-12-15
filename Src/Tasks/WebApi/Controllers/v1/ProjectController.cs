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
        public async Task<IActionResult> AddNewProject([FromBody] NewProjectModel newProject)
        {
            try
            {
                // Check validity of the request
                var claimsManager = HttpContext.User;
                if(!claimsManager.HasClaim(c => c.Type == "uid"))
                {
                    return Unauthorized(new HttpResponse<object>(false, null, "Token provided is invalid because there is no valid confidential claim"));
                }
                // Extract uid from token
                long uid;
                try
                {
                    uid = long.Parse(claimsManager.Claims.FirstOrDefault(c => c.Type == "uid").Value);
                } catch (Exception)
                {
                    return Unauthorized(new HttpResponse<object>(false, null, "Token provided is invalid because the value for the claim is invalid"));
                }

                // Carry on with the business logic
                ProjectResponseModel addedProject = await _projectService.AddNewProject(uid, newProject);
                return Ok(new HttpResponse<ProjectResponseModel>(true, addedProject, message: "Successfully added project"));
            }
            catch (Exception ex)
            {
                if (ex is ProjectServiceException exception)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("A problem occurred when processing the content of your request, please recheck your request params: ");
                    sb.AppendLine(exception.Message);
                    return StatusCode(exception.StatusCode, new HttpResponse<object>(false, null, sb.ToString()));
                }
                return StatusCode(500, new HttpResponse<Exception>(false, ex, "Server encountered an exception"));
            }
        }

        [HttpGet("projects")]
        public async Task<IActionResult> GetAllProjects([FromQuery] GetAllProjectsModel model)
        {
            try
            {
                //Check validity of the token
                if (model.UserID != null)
                {
                    return BadRequest(new HttpResponse<object>(false, null, "Found illegal parameter UserID in query, we refuse to carry on with your request"));
                }
                var claimsManager = HttpContext.User;
                if (!claimsManager.HasClaim(c => c.Type == "uid"))
                {
                    return Unauthorized(new HttpResponse<object>(false, null, "Token provided is invalid because there is no valid confidential claim"));
                }
                // Extract uid from token
                long uid;
                try
                {
                    uid = long.Parse(claimsManager.Claims.FirstOrDefault(c => c.Type == "uid").Value);
                }
                catch (Exception)
                {
                    return Unauthorized(new HttpResponse<object>(false, null, "Token provided is invalid because the value for the claim is invalid"));
                }

                // If passes all tests, then we submit it to the service layer
                model.UserID = uid;
                // Carry on with the business logic
                IEnumerable<ProjectResponseModel> projectParticipations = await _projectService.GetAllProjects(model);
                return Ok(new HttpResponse<IEnumerable<ProjectResponseModel>>(true, projectParticipations, message: "Successfully fetched projects of user"));
            }
            catch (Exception ex)
            {
                if (ex is ProjectServiceException exception)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("A problem occurred when processing the content of your request, please recheck your request params: ");
                    sb.AppendLine(exception.Message);
                    return StatusCode(exception.StatusCode, new HttpResponse<object>(false, null, sb.ToString()));
                }
                return StatusCode(500, new HttpResponse<Exception>(false, ex, "Server encountered an exception"));
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
                    return Unauthorized(new HttpResponse<object>(false, null, "Token provided is invalid because there is no valid confidential claim"));
                }
                // Extract uid from token
                long uid;
                try
                {
                    uid = long.Parse(claimsManager.Claims.FirstOrDefault(c => c.Type == "uid").Value);
                }
                catch (Exception)
                {
                    return Unauthorized(new HttpResponse<object>(false, null, "Token provided is invalid because the value for the claim is invalid"));
                }

                // If passes all tests, then we submit it to the service layer
                GetOneProjectModel model = new GetOneProjectModel()
                {
                    ProjectId = projectId,
                    UserId = uid,
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
                    return StatusCode(exception.StatusCode, new HttpResponse<object>(false, null, sb.ToString()));
                }
                return StatusCode(500, new HttpResponse<Exception>(false, ex, "Server encountered an exception"));
            }
        }

        [HttpPatch("project/{projectId}")]
        public async Task<IActionResult> UpdateExistingProject(int projectId, [FromBody] UpdateProjectInfoModel model)
        {
            try
            {
                //Check validity of the token
                var claimsManager = HttpContext.User;
                if (!claimsManager.HasClaim(c => c.Type == "uid"))
                {
                    return Unauthorized(new HttpResponse<object>(false, null, "Token provided is invalid because there is no valid confidential claim"));
                }
                // Extract uid from token
                long uid;
                try
                {
                    uid = long.Parse(claimsManager.Claims.FirstOrDefault(c => c.Type == "uid").Value);
                }
                catch (Exception)
                {
                    return Unauthorized(new HttpResponse<object>(false, null, "Token provided is invalid because the value for the claim is invalid"));
                }

                // If passes all tests, then we submit it to the service layer
                // Carry on with the business logic
                ProjectResponseModel participatedProject = await _projectService.UpdateProjectInfo(projectId, uid, model);
                return Ok(new HttpResponse<ProjectResponseModel>(true, participatedProject, message: "Successfully patched specified project of user"));
            }
            catch (Exception ex)
            {
                if (ex is ProjectServiceException exception)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("A problem occurred when processing the content of your request, please recheck your request params: ");
                    sb.AppendLine(exception.Message);
                    return StatusCode(exception.StatusCode, new HttpResponse<object>(false, null, sb.ToString()));
                }
                return StatusCode(500, new HttpResponse<Exception>(false, ex, "Server encountered an exception"));
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
                    return Unauthorized(new HttpResponse<object>(false, null, "Token provided is invalid because there is no valid confidential claim"));
                }
                // Extract uid from token
                long uid;
                try
                {
                    uid = long.Parse(claimsManager.Claims.FirstOrDefault(c => c.Type == "uid").Value);
                }
                catch (Exception)
                {
                    return Unauthorized(new HttpResponse<object>(false, null, "Token provided is invalid because the value for the claim is invalid"));
                }

                // If passes all tests, then we submit it to the service layer
                // Carry on with the business logic
                ProjectResponseModel participatedProject = await _projectService.SoftDeleteExistingProject(projectId, uid);
                return Ok(new HttpResponse<ProjectResponseModel>(true, participatedProject, message: "Successfully patched specified project of user"));
            }
            catch (Exception ex)
            {
                if (ex is ProjectServiceException exception)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("A problem occurred when processing the content of your request, please recheck your request params: ");
                    sb.AppendLine(exception.Message);
                    return StatusCode(exception.StatusCode, new HttpResponse<object>(false, null, sb.ToString()));
                }
                return StatusCode(500, new HttpResponse<Exception>(false, ex, "Server encountered an exception"));
            }
        }
    }
}
