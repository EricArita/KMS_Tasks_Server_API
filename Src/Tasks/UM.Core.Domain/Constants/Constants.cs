using static UM.Core.Domain.Constants.Enums;

namespace UM.Core.Domain.Constants
{
    public static class DefaultUserConstants
    {
        public const string DefaultUsername = "admin";
        public const string DefaultEmail = "admin@test.com";
        public const string DefaultPassword = "admin@123";
        public const UserRoles DefaultRole = UserRoles.Administrator;
    }

    public static class UserRelatedErrorsConstants
    {
        public const string USER_NOT_FOUND = "Cannot locate a valid user from the claim provided";
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
    }

    public static class InternalServerErrorsConstants
    {
        public const string DATABASE_INTEGRITY_NOT_MAINTAINED = "Inconsistency in database. Executing query reports so...";
    }

    public static class ErrorLoggingMessagesConstants
    {
        public const string PROJECT_SERVICE_ERROR_LOG_MESSAGE = "An error occurred when using ProjectService";
        public const string TASK_SERVICE_ERROR_LOG_MESSAGE = "An error occurred when using TaskService";
    }
}