using Core.Application.Helper;
using Core.Application.Helper.Exceptions.Participation;
using Core.Application.Interfaces;
using Core.Application.Models.Participation;
using Core.Application.Models.Participation.GETSpecificResponses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Controllers.v1.Utils;

namespace WebApi.Controllers.v1
{
    [Area("participation-management")]
    public class ParticipationController : BaseController
    {
        private readonly IParticipationService _participationService;

        public ParticipationController(IParticipationService participationService)
        {
            _participationService = participationService; 
        }

        [HttpPost("participation")]
        public async Task<IActionResult> AddNewParticipation([FromBody] NewParticipationModel newParticipation)
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
                ParticipationResponseModel addedParticipation = await _participationService.AddNewParticipation(uid.Value, newParticipation);
                return Ok(new HttpResponse<ParticipationResponseModel>(true, addedParticipation, message: "Successfully added participation"));
            }
            catch (Exception ex)
            {
                if (ex is ParticipationServiceException exception)
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

        [HttpGet("participations")]
        public async Task<IActionResult> GetAllProjects([FromQuery] GetAllParticipationsModel model)
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

                // Carry on with the business logic
                IGetAllParticipations_ResponseModel projectParticipations = await _participationService.GetAllParticipations(uid.Value ,model);
                return Ok(new HttpResponse<IGetAllParticipations_ResponseModel>(true, projectParticipations, message: "Successfully fetched participations of user"));
            }
            catch (Exception ex)
            {
                if (ex is ParticipationServiceException exception)
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

        [HttpDelete("participation")]
        public async Task<IActionResult> DeleteExistingParticipation([FromQuery] DeleteParticipationModel model)
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
                ParticipationResponseModel projectParticipation = await _participationService.DeleteExistingParticipation(uid.Value, model);
                return Ok(new HttpResponse<ParticipationResponseModel>(true, projectParticipation, message: "Successfully deleted specified participation of user"));
            }
            catch (Exception ex)
            {
                if (ex is ParticipationServiceException exception)
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
