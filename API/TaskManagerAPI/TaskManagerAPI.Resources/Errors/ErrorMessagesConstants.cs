namespace TaskManagerAPI.Resources.Errors
{
    /// <summary>
    /// Error Messages List, each message has the same name as the error code.
    /// Instead of having so many error messages, each regions should have its own class
    /// </summary>
    public class ErrorsMessagesConstants
    {
        #region API Layer

        public const string UNKNOWN_ERROR_API = "Oops! Something went wrong!\nHelp us improve your experience by sending an error report";

        #endregion API Layer

        #region BL Layer

        public const string INVALID_EMAIL_OR_PASSWORD = "The email or password provided is invalid";
        public const string CURRENT_USER_ID_NOT_FOUND = "The token provided is not valid";
        public const string TASK_ID_NOT_FOUND = "The task requested doesn't exist";
        public const string USER_ID_NOT_FOUND = "The user requested doesn't exist";
        public const string EMAIL_ALREADY_USED = "The email is already been used. Please try another one";
        public const string USER_DISABLED = "The user is disabled";
        public const string USER_LOCKED = "The user is locked";

        #endregion BL Layer

        #region Repository Layer

        public const string UNABLE_TO_SAVE_CHANGES_IN_TASK_TABLE = "We're unable to do the operation requested";
        public const string UNABLE_TO_SAVE_CHANGES_IN_ACCOUNT_TABLE = "We're unable to do the operation requested";

        #endregion Repository Layer
    }
}