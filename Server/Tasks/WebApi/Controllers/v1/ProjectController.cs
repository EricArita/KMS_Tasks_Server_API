using Core.Application.Helper;
using Core.Application.Helper.Exceptions.Project;
using Core.Application.Interfaces;
using Core.Application.Models;
using Core.Domain.DbEntities;
using Infrastructure.Persistence.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers.v1
{
    public class ProjectController : BaseController
    {
        private IProjectService _projectService;
        private ILogger<ProjectController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProjectController(IProjectService projectService, UserManager<ApplicationUser> userManager, ILogger<ProjectController> logger)
        {
            _projectService = projectService;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpPost("project")]
        public async Task<IActionResult> AddNewProject(NewProjectModel newProject)
        {
            try
            {
                // Check validity of the request
                if (newProject.CreatedBy != null) {
                    return Unauthorized(new Response<string>(false, "Unauthorized creation of project"));
                }
                var claimsManager = HttpContext.User;
                if(!claimsManager.HasClaim(c => c.Type == "uid"))
                {
                    return Unauthorized(new Response<string>(false, "Token provided is invalid because there is no valid confidential claim"));
                }
                // Extract uid from token
                int uid;
                try
                {
                    uid = int.Parse(claimsManager.Claims.FirstOrDefault(c => c.Type == "uid").Value);
                } catch (Exception)
                {
                    return Unauthorized(new Response<string>(false, "Token provided is invalid because the value for the claim is invalid"));
                }
                //Check if uid is valid
                ApplicationUser validUser = _userManager.Users.FirstOrDefault(e => e.UserId == uid);
                if(validUser == null)
                {
                    return BadRequest(new Response<string>(false, "Cannot locate a valid user from the claim provided"));
                }
                newProject.CreatedBy = uid;

                // Carry on with the business logic
                Project addedProject = await _projectService.AddNewProject(newProject);
                return Ok(new Response<Project>(true, addedProject, message: "Successfully added project"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred while processing request", ex.Message, ex.StackTrace);
                if (ex is ProjectServiceException)
                {
                    return StatusCode(400, new Response<string>(false, "A problem occurred when processing the content of your request, please recheck your request params"));
                }
                return StatusCode(500, new Response<Exception>(false, ex, "Server encountered an exception"));
            }
        }

        [HttpGet("projects")]
        public async Task<IActionResult> GetAllProjects(int userId)
        {
            try
            {
                //Check validity of the token
                var claimsManager = HttpContext.User;
                if (!claimsManager.HasClaim(c => c.Type == "uid"))
                {
                    return Unauthorized(new Response<string>(false, "Token provided is invalid because there is no valid confidential claim"));
                }
                // Extract uid from token
                int uid;
                try 
                {
                    uid = int.Parse(claimsManager.Claims.FirstOrDefault(c => c.Type == "uid").Value);
                } catch (Exception)
                {
                    return Unauthorized(new Response<string>(false, "Token provided is invalid because the value for the confidential claim is invalid"));
                }
                // Check if uid is valid or not
                ApplicationUser validUser = _userManager.Users.FirstOrDefault(e => e.UserId == uid);
                if (validUser == null)
                {
                    return BadRequest(new Response<string>(false, "Cannot locate a valid user from the claim provided"));
                }

                // If passes all tests, then we submit it to the service layer
                GetAllProjectsModel model = new GetAllProjectsModel()
                {
                    UserID = uid,
                };
                // Carry on with the business logic
                IEnumerable<object> projectParticipations = await _projectService.GetAllProjects(model);
                return Ok(new Response<IEnumerable<object>>(true, projectParticipations, message: "Successfully fetched projects of user"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred while processing request", ex.Message, ex.StackTrace);
                if (ex is ProjectServiceException)
                {
                    return StatusCode(400, new Response<string>(false, "A problem occurred when processing the content of your request, please recheck your request params"));
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
                    return Unauthorized(new Response<string>(false, "Token provided is invalid because there is no valid confidential claim"));
                }
                // Extract uid from token
                int uid;
                try
                {
                    uid = int.Parse(claimsManager.Claims.FirstOrDefault(c => c.Type == "uid").Value);
                }
                catch (Exception)
                {
                    return Unauthorized(new Response<string>(false, "Token provided is invalid because the value for the confidential claim is invalid"));
                }
                // Check if uid is valid or not
                ApplicationUser validUser = _userManager.Users.FirstOrDefault(e => e.UserId == uid);
                if (validUser == null)
                {
                    return BadRequest(new Response<string>(false, "Cannot locate a valid user from the claim provided"));
                }

                // If passes all tests, then we submit it to the service layer
                GetOneProjectModel model = new GetOneProjectModel()
                {
                    ProjectId = projectId,
                    UserId = uid,
                };
                // Carry on with the business logic
                object participatedProject = await _projectService.GetOneProject(model);
                return Ok(new Response<object>(true, participatedProject, message: "Successfully fetched specified project of user"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred while processing request", ex.Message, ex.StackTrace);
                if (ex is ProjectServiceException)
                {
                    return StatusCode(400, new Response<string>(false, "A problem occurred when processing the content of your request, please recheck your request params"));
                }
                return StatusCode(500, new Response<Exception>(false, ex, "Server encountered an exception"));
            }
        }
    }
}
