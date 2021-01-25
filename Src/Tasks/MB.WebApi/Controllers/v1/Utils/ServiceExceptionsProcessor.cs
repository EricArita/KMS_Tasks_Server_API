using System.Collections.Generic;
using MB.Core.Domain.Constants;

namespace MB.WebApi.Controllers.v1.Utils
{
    // This class produces the corresponding status code from a passed in string
    public static class ServiceExceptionsProcessor
    {
        private static readonly Dictionary<string, uint> statusCodeDictionary;

        static ServiceExceptionsProcessor(){
            statusCodeDictionary = new Dictionary<string, uint>() {
                [UserRelatedErrorsConstants.USER_NOT_FOUND] = 404,
                [UserRelatedErrorsConstants.PASSWORD_CHANGE_ERROR] = 400,
                [UserRelatedErrorsConstants.MISSING_CURRENT_PASSWORD_WHEN_CHANGING_PASSWORD] = 400,

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
                [ProjectParticipationRelatedErrorsConstants.QUERIED_USER_HAS_NO_PARTICIPATIONS_IN_QUERIED_PROJECT] = 400,
                [ProjectParticipationRelatedErrorsConstants.PARTICIPATION_CREATOR_DONT_HAVE_THE_RIGHTS] = 403,
                [ProjectParticipationRelatedErrorsConstants.CANNOT_CREATE_PARTICIPATION_WITH_NONE_AS_A_ROLE] = 400,
                [ProjectParticipationRelatedErrorsConstants.CANNOT_CREATE_PARTICIPATION_WITH_OWNER_AS_A_ROLE] = 400,
                [ProjectParticipationRelatedErrorsConstants.CANNOT_RECREATE_AN_EXISTING_PARTICIPATION] = 400,
                [ProjectParticipationRelatedErrorsConstants.CANNOT_LOCATE_AN_EXISTING_PARTICIPATION_FOR_REMOVAL] = 400,
                [ProjectParticipationRelatedErrorsConstants.CANNOT_REMOVE_THE_OWNER_FROM_HIS_OWN_PROJECT] = 403,
                [ProjectParticipationRelatedErrorsConstants.PARTICIPATION_REMOVER_DONT_HAVE_THE_RIGHTS] = 403,
                [ProjectParticipationRelatedErrorsConstants.THERE_IS_NO_PARTICIPATION_WITH_A_NONE_ROLE] = 400,

                [InternalServerErrorsConstants.DATABASE_INTEGRITY_NOT_MAINTAINED] = 500,
                [InternalServerErrorsConstants.GET_ALL_PARTICIPATIONS_STRATEGY_INVALID] = 500,
            };
        }

        public static uint? GetStatusCode(string input)
        {
            if (input == null) return null;
            uint? result = statusCodeDictionary[input];
            return result;
        }
    }
}
