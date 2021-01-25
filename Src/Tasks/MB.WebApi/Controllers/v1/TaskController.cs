using MB.Core.Application.Helper;
using MB.Core.Application.Helper.Exceptions.Task;
using MB.Core.Application.Interfaces;
using MB.Core.Application.Models;
using MB.Core.Application.Models.Task;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MB.WebApi.Controllers.v1.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace MB.WebApi.Controllers.v1
{
    [Area("task-management")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TaskController : BaseController
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost("task")]
        public async Task<IActionResult> AddNewTask([FromBody] NewTaskModel newTask)
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
                TaskResponseModel addedTask = await _taskService.AddNewTask(uid.Value, newTask);
                return Ok(new HttpResponse<TaskResponseModel>(true, addedTask, message: "Successfully added task"));
            }
            catch (Exception ex)
            {
                if (ex is TaskServiceException exception)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("A problem occurred when processing the content of your request, please recheck your request params: ");
                    sb.AppendLine(exception.Message);
                    uint? statusCode = ServiceExceptionsProcessor.GetStatusCode(exception.Message);
                    if (statusCode != null && statusCode.HasValue)
                    {
                        return StatusCode((int)statusCode.Value, new HttpResponse<object>(false, null, sb.ToString()));
                    }
                }
                return StatusCode(500, new HttpResponse<Exception>(false, ex, "Server encountered an exception"));
            }
        }

        [HttpGet("tasks")]
        public async Task<IActionResult> GetAllTasks([FromQuery] GetAllTasksRequestModel model)
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
                GetAllTasksModel serviceModel = new GetAllTasksModel()
                {
                    UserId = uid,
                    CategoryType = model.CategoryType,
                    ProjectId = model.ProjectId
                };
                // Carry on with the business logic
                IEnumerable<TaskResponseModel> tasks = await _taskService.GetAllTasks(serviceModel);
                return Ok(new HttpResponse<IEnumerable<TaskResponseModel>>(true, tasks, message: "Successfully fetched tasks of user"));
            }
            catch (Exception ex)
            {
                if (ex is TaskServiceException exception)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("A problem occurred when processing the content of your request, please recheck your request params: ");
                    sb.AppendLine(exception.Message);
                    uint? statusCode = ServiceExceptionsProcessor.GetStatusCode(exception.Message);
                    if (statusCode != null && statusCode.HasValue)
                    {
                        return StatusCode((int)statusCode.Value, new HttpResponse<object>(false, null, sb.ToString()));
                    }
                }
                return StatusCode(500, new HttpResponse<Exception>(false, ex, "Server encountered an exception"));
            }
        }

        [HttpGet("task/{taskId}")]
        public async Task<IActionResult> GetAParticularTask(long taskId)
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
                GetOneTaskModel model = new GetOneTaskModel()
                {
                    TaskId = taskId,
                    UserId = uid.Value,
                };
                // Carry on with the business logic
                TaskResponseModel participatedTask = await _taskService.GetOneTask(model);
                return Ok(new HttpResponse<TaskResponseModel>(true, participatedTask, message: "Successfully fetched specified project of user"));
            }
            catch (Exception ex)
            {
                if (ex is TaskServiceException exception)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("A problem occurred when processing the content of your request, please recheck your request params: ");
                    sb.AppendLine(exception.Message);
                    uint? statusCode = ServiceExceptionsProcessor.GetStatusCode(exception.Message);
                    if (statusCode != null && statusCode.HasValue)
                    {
                        return StatusCode((int)statusCode.Value, new HttpResponse<object>(false, null, sb.ToString()));
                    }
                }
                return StatusCode(500, new HttpResponse<Exception>(false, ex, "Server encountered an exception"));
            }
        }

        [HttpPatch("task/{taskId}")]
        public async Task<IActionResult> UpdateAnExistingTask(long taskId, [FromBody] UpdateTaskInfoModel model)
        {
            try
            {
                //Check validity of the token
                var claimsManager = HttpContext.User;
                long? uid = null;
                try
                {
                    uid = GetUserId(claimsManager);
                } catch (Exception e)
                {
                    return Unauthorized(e.Message);
                }

                if(!uid.HasValue)
                {
                    return Unauthorized("Unauthorized individuals cannot access this route");
                }

                // If passes all tests, then we submit it to the service layer
                // Carry on with the business logic
                TaskResponseModel updatedTask = await _taskService.UpdateTaskInfo(taskId, uid.Value, model);
                return Ok(new HttpResponse<TaskResponseModel>(true, updatedTask, message: "Successfully patched specified task of user"));
            }
            catch (Exception ex)
            {
                if (ex is TaskServiceException exception)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("A problem occurred when processing the content of your request, please recheck your request params: ");
                    sb.AppendLine(exception.Message);
                    uint? statusCode = ServiceExceptionsProcessor.GetStatusCode(exception.Message);
                    if (statusCode != null && statusCode.HasValue)
                    {
                        return StatusCode((int)statusCode.Value, new HttpResponse<object>(false, null, sb.ToString()));
                    }
                }
                return StatusCode(500, new HttpResponse<Exception>(false, ex, "Server encountered an exception"));
            }
        }

        [HttpDelete("task/{taskId}")]
        public async Task<IActionResult> DeleteExistingTask(long taskId)
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
                TaskResponseModel participatedTask = await _taskService.SoftDeleteExistingTask(taskId, uid.Value);
                return Ok(new HttpResponse<TaskResponseModel>(true, participatedTask, message: "Successfully deleted specified task of user"));
            }
            catch (Exception ex)
            {
                if (ex is TaskServiceException exception)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("A problem occurred when processing the content of your request, please recheck your request params: ");
                    sb.AppendLine(exception.Message);
                    uint? statusCode = ServiceExceptionsProcessor.GetStatusCode(exception.Message);
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
