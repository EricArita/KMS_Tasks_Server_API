using Core.Application.Helper;
using Core.Application.Interfaces;
using Core.Application.Models;
using Core.Domain.DbEntities;
using Infrastructure.Persistence.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private Logger _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProjectController(IProjectService projectService, UserManager<ApplicationUser> userManager)
        {
            _projectService = projectService;
            _logger = NLoggerService.GetLogger();
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
                _logger.Error(ex);
                return StatusCode(500, new Response<Exception>(false, ex, "Server encountered an exception"));
            }
        }

        [HttpGet("{userId}/projects")]
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
                //Check if userId provided is valid or not
                if(userId != uid)
                {
                    return BadRequest(new Response<string>(false, "UserId provided doesn't match your claim"));
                }
                else
                {
                    // Check if uid is valid or not
                    ApplicationUser validUser = _userManager.Users.FirstOrDefault(e => e.UserId == uid);
                    if (validUser == null)
                    {
                        return BadRequest(new Response<string>(false, "Cannot locate a valid user from the claim provided"));
                    }
                }
                // If passes all tests, then we submit it to the service layer
                GetAllProjectsModel model = new GetAllProjectsModel()
                {
                    UserID = uid,
                };
                // Carry on with the business logic
                IEnumerable<Project> results = await _projectService.GetAllProjects(model);
                return Ok(new Response<IEnumerable<Project>>(true, results, message: "Successfully fetched projects of user"));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return StatusCode(500, new Response<Exception>(false, ex, "Server encountered an exception"));
            }
        }
    }
}
