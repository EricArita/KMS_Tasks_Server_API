using Core.Application.Helper;
using Core.Application.Interfaces;
using Core.Application.Models;
using Core.Domain.DbEntities;
using Infrastructure.Persistence.Services;
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

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
            _logger = NLoggerService.GetLogger();
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
                var currentUser = HttpContext.User;
                if (!currentUser.HasClaim(c => c.Type == "uid"))
                {
                    return BadRequest(new Response<string>(false, "Cannot add project for invalid user"));
                }
                newProject.CreatedBy = currentUser.Claims.FirstOrDefault(c => c.Type == "uid").Value;
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
                // Check validity of the request
                var currentUser = HttpContext.User;
                if (!currentUser.HasClaim(c => c.Type == "uid"))
                {
                    return BadRequest(new Response<string>(false, "Cannot add project for invalid user"));
                }
                GetAllProjectsModel model = new GetAllProjectsModel()
                {
                    UserID = currentUser.Claims.FirstOrDefault(c => c.Type == "uid").Value,
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
