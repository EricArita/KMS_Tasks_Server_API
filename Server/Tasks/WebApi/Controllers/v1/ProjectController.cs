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
                if (newProject.CreatedBy != null) throw new Exception("Unauthorized creation of project found"); 
                var currentUser = HttpContext.User;
                if (!currentUser.HasClaim(c => c.Type == "uid")) throw new Exception("Cannot add project for invalid user");
                newProject.CreatedBy = currentUser.Claims.FirstOrDefault(c => c.Type == "uid").Value;
                Project addedProject = await _projectService.AddNewProject(newProject);
                return Ok(new Response<Project>(addedProject));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return StatusCode(500, ex);
            }
        }

        [HttpGet("{userId}/projects")]
        public async Task<IActionResult> GetAllProjects(int userId)
        {
            try
            {
                var currentUser = HttpContext.User;
                if (!currentUser.HasClaim(c => c.Type == "uid")) throw new Exception("Cannot add project for invalid user");
                GetAllProjectsModel model = new GetAllProjectsModel()
                {
                    UserID = currentUser.Claims.FirstOrDefault(c => c.Type == "uid").Value,
                };
                IEnumerable<Project> results = await _projectService.GetAllProjects(model);
                return Ok(new Response<IEnumerable<Project>>(results));
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return StatusCode(500, ex);
            }
        }
    }
}
