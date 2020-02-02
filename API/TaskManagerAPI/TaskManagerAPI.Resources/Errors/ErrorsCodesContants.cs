namespace TaskManagerAPI.Resources.Errors
{
    /// <summary>
    /// TODO: Reestructure using enums type
    /// Error Codes List with specific format by layer, Instead of having much errors as constants in this class
    /// Separate classes for each regions should be created
    /// </summary>
    public static class ErrorsCodesContants
    {
        // XYZZZ
        // X LAYER
        // Y ERROR Type
        //// Y=4 Bad input data
        //// Y=5 Internal Error
        // ZZZ Specific Error Code

        #region API Layer

        public const string UNKNOWN_ERROR_API = "05001";

        #endregion API Layer

        #region BL Layer

        public const string INVALID_EMAIL_OR_PASSWORD = "14001";
        public const string CURRENT_USER_ID_NOT_FOUND = "14002";
        public const string TASK_ID_NOT_FOUND = "14003";
        public const string USER_ID_NOT_FOUND = "14004";
        public const string EMAIL_ALREADY_USED = "14005";
        public const string USER_DISABLED = "14006";
        public const string USER_LOCKED = "14007";

        #endregion BL Layer

        #region Repository Layer

        public const string UNABLE_TO_SAVE_CHANGES_IN_TASK_TABLE = "25001";
        public const string UNABLE_TO_SAVE_CHANGES_IN_ACCOUNT_TABLE = "25002";

        #endregion Repository Layer
    }
}