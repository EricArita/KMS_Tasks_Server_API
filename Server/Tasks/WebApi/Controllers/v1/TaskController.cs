using Core.Application.Helper;
using Core.Application.Interfaces;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private IUnitOfWork unitOfWork;

        public TaskController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpPost("addtask")]
        public IActionResult AddNewTask(TaskRequestModel newTask)
        {
            this.unitOfWork.Repository<TaskRepository>().AddNewTask(newTask);
            var res = this.unitOfWork.SaveChanges();
            if (res != 0) return Ok();
            else return BadRequest(new Response<bool>(false, "Some errors has occured in server!"));
        }
    }
}
