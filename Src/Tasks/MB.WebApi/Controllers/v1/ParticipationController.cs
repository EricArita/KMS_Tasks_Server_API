﻿using MB.Core.Application.Helper;
using MB.Core.Application.Helper.Exceptions.Participation;
using MB.Core.Application.Interfaces;
using MB.Core.Application.Models.Participation;
using MB.Core.Application.Models.Participation.GETSpecificResponses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using System.Threading.Tasks;
using MB.WebApi.Controllers.v1.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using MB.Core.Domain.DbEntities;
using Microsoft.AspNetCore.SignalR;
using MB.WebApi.Hubs.v1;
using MB.Core.Application.Interfaces.Misc;

namespace MB.WebApi.Controllers.v1
{
    [Area("participation-management")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ParticipationController : BaseController
    {
        private readonly IParticipationService _participationService;

        public ParticipationController(IParticipationService participationService, UserManager<ApplicationUser> userManager) : base(userManager)
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
        public async Task<IActionResult> GetAllParticipations([FromQuery] GetAllParticipationsModel model)
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
                await _participationService.DeleteExistingParticipation(uid.Value, model);
                return Ok(new HttpResponse<object>(true, null, message: "Successfully deleted specified participation(s) of user"));
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
