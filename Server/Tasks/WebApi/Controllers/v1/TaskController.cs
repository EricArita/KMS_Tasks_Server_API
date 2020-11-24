using Core.Application.Helper;
using Core.Application.Interfaces;
using Core.Domain.DbEntities;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApi.Models;

namespace WebApi.Controllers.v1
{
    public class TaskController : BaseController
    {
        private IUnitOfWork unitOfWork;

        public TaskController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpPost("task")]
        public IActionResult AddNewTask(NewTaskModel newTask)
        {
            this.unitOfWork.Repository<TaskRepository>().AddNewTask(newTask);
            var res = this.unitOfWork.SaveChanges();
            if (res != 0) return Ok();
            else return BadRequest(new Response<bool>(false, "Some errors has occured in server!"));
        }

        [HttpGet("{userId}/tasks")]
        public IActionResult GetAllTasks(int userId, byte category)
        {
            var listTasks = unitOfWork.Repository<TaskRepository>().GetAllTasks(userId, category);
            return Ok(new Response<IEnumerable<Tasks>>(listTasks));
        }
    }
}
