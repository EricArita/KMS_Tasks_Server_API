using static MB.Core.Domain.Constants.Enums;

namespace MB.Core.Domain.Constants
{
    public static class DefaultUserConstants
    {
        public const string DefaultUsername = "admin";
        public const string DefaultEmail = "admin@test.com";
        public const string DefaultPassword = "admin@123";
        public const UserRoles DefaultRole = UserRoles.Administrator;
    }

    // Errors

    public static class UserRelatedErrorsConstants
    {
        public const string USER_NOT_FOUND = "Cannot locate a valid user from the claim provided";
        public const string MISSING_CURRENT_PASSWORD_WHEN_CHANGING_PASSWORD = "You need to provide your current password to change your password";
        public const string PASSWORD_CHANGE_ERROR = "Errors happened when trying to modify the password";
    }

    public static class ProjectRelatedErrorsConstants
    {
        public const string PARENT_PROJECT_NOT_FOUND = "Cannot find a single instance of a parent project from the infos you provided";
        public const string PROJECT_NOT_FOUND = "Cannot find a single instance of a project from the infos you provided";
        public const string CANNOT_SET_PARENT_PROJECT_TOBE_ITSELF = "Cannot set a project to be its own parent";
        public const string ACCESS_TO_PROJECT_IS_FORBIDDEN = "You shan't modify this project";
    }

    public static class TaskRelatedErrorsConstants
    {
        public const string TASK_NOT_FOUND = "Cannot find a single instance of a task from the infos you provided";
        public const string CANNOT_SET_PARENT_TASK_TOBE_ITSELF = "Cannot set a task to be its own parent";
        public const string CANNOT_SET_NEW_PARENT_PROJECT_TOBE_THE_OLD_VALUE = "Cannot set a task to belong to its current project, it already is";
        public const string PARENT_TASK_NOT_FOUND = "Cannot find a single instance of the parent task from the infos you provided";
        public const string PARENT_TASK_OF_A_TASK_ISFROM_ANOTHER_PROJECT = "Your parent task of this task could not be from another project";
        public const string ASSIGNED_BY_FIELD_INVALID = "Cannot locate a valid user for assignedBy field from the data provided";
        public const string ASSIGNED_FOR_FIELD_INVALID = "Cannot locate a valid user for assignedFor field from the data provided";
        public const string ACCESS_TO_TASK_IS_FORBIDDEN = "You shan't modify this task because you are forbidden from modifying the project's related to";
    }

    public static class ProjectParticipationRelatedErrorsConstants
    {
        public const string PROJECT_PARTICIPATION_NOT_FOUND = "Cannot find any project of such that you participated in";
        public const string QUERIED_USER_HAS_NO_PARTICIPATIONS_IN_QUERIED_PROJECT = "Queried user has no participation in queried project";
        // Create participation errors
        public const string PARTICIPATION_CREATOR_DONT_HAVE_THE_RIGHTS = "Creator of the participation don't have the rights to perform the action";
        public const string CANNOT_CREATE_PARTICIPATION_WITH_NONE_AS_A_ROLE = "A participation must have one valid role to be created";
        public const string CANNOT_CREATE_PARTICIPATION_WITH_OWNER_AS_A_ROLE = "Owner role can only be given to the one who created the project";
        public const string CANNOT_RECREATE_AN_EXISTING_PARTICIPATION = "The participation you want to create already exist";
        // Delete participation errors
        public const string THERE_IS_NO_PARTICIPATION_WITH_A_NONE_ROLE = "There are no participation with none as a role";
        public const string PARTICIPATION_REMOVER_DONT_HAVE_THE_RIGHTS = "Remover of the participation don't have the rights to perform the action";
        public const string CANNOT_LOCATE_AN_EXISTING_PARTICIPATION_FOR_REMOVAL = "Cannot find an existing participation from the infos you provide to remove";
        public const string CANNOT_REMOVE_THE_OWNER_FROM_HIS_OWN_PROJECT = "Cannot remove the owner from the project, this action is forbidden";
    }

    public static class InternalServerErrorsConstants
    {
        public const string DATABASE_INTEGRITY_NOT_MAINTAINED = "Inconsistency in database. Executing query reports so...";
        public const string GET_ALL_PARTICIPATIONS_STRATEGY_INVALID = "You cannot provide an invalid strategy for getting all participations";
    }

    public static class ErrorLoggingMessagesConstants
    {
        public const string PROJECT_SERVICE_ERROR_LOG_MESSAGE = "An error occurred while using ProjectService";
        public const string TASK_SERVICE_ERROR_LOG_MESSAGE = "An error occurred while using TaskService";
        public const string PARTICIPATION_SERVICE_ERROR_LOG_MESSAGE = "An error occured while using ParticipationService";
        public const string USER_SERVICE_ERROR_LOG_MESSAGE = "An error occurred while using UserService";
    }
}