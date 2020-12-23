﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain.Constants;

namespace WebApi.Controllers.v1.Utils
{
    // This class produces the corresponding status code from a passed in string
    public static class ServiceExceptionsProcessor
    {
        private static Dictionary<string, uint> statusCodeDictionary;

        static ServiceExceptionsProcessor(){
            statusCodeDictionary = new Dictionary<string, uint>() {
                [UserRelatedErrorsConstants.USER_NOT_FOUND] = 404,

                [ProjectRelatedErrorsConstants.ACCESS_TO_PROJECT_IS_FORBIDDEN] = 403,
                [ProjectRelatedErrorsConstants.CANNOT_SET_PARENT_PROJECT_TOBE_ITSELF] = 400,
                [ProjectRelatedErrorsConstants.PARENT_PROJECT_NOT_FOUND] = 404,
                [ProjectRelatedErrorsConstants.PROJECT_NOT_FOUND] = 404,

                [TaskRelatedErrorsConstants.ACCESS_TO_TASK_IS_FORBIDDEN] = 403,
                [TaskRelatedErrorsConstants.ASSIGNED_BY_FIELD_INVALID] = 404,
                [TaskRelatedErrorsConstants.ASSIGNED_FOR_FIELD_INVALID] = 404,
                [TaskRelatedErrorsConstants.CANNOT_SET_NEW_PARENT_PROJECT_TOBE_THE_OLD_VALUE] = 400,
                [TaskRelatedErrorsConstants.CANNOT_SET_PARENT_TASK_TOBE_ITSELF] = 400,
                [TaskRelatedErrorsConstants.PARENT_TASK_NOT_FOUND] = 404,
                [TaskRelatedErrorsConstants.PARENT_TASK_OF_A_TASK_ISFROM_ANOTHER_PROJECT] = 400,
                [TaskRelatedErrorsConstants.TASK_NOT_FOUND] = 404,

                [ProjectParticipationRelatedErrorsConstants.PROJECT_PARTICIPATION_NOT_FOUND] = 404,

                [InternalServerErrorsConstants.DATABASE_INTEGRITY_NOT_MAINTAINED] = 500,
            };
        }

        public static uint? getStatusCode(string input)
        {
            if (input == null) return null;
            uint? result = statusCodeDictionary[input];
            return result;
        }
    }
}
