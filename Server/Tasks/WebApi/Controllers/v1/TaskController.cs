using Core.Application.Helper;
using Core.Application.Interfaces;
using Core.Application.Models;
using Core.Domain.DbEntities;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebApi.Controllers.v1
{
    public class TaskController : BaseController
    {
        private IUnitOfWork unitOfWork;
        private ITaskService taskService;

        public TaskController(IUnitOfWork unitOfWork, ITaskService taskService)
        {
            this.unitOfWork = unitOfWork;
            this.taskService = taskService;
        }

        [HttpPost("task")]
        public IActionResult AddNewTask(NewTaskModel newTask)
        {
            var res = taskService.AddNewTask(newTask);
            if (res != 0) return Ok();
            else return BadRequest(new Response<bool>(false, message:"Some errors has occured in server!"));
        }

        [HttpGet("{userId}/tasks")]
        public IActionResult GetAllTasks(int userId, byte category)
        {
            var listTasks = taskService.GetAllTasks(userId, category);
            return Ok(new Response<IEnumerable<Tasks>>(true, data: listTasks));
        }
    }
}
