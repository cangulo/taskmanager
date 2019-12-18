namespace TaskManagerAPI.Resources.Constants
{
    /// <summary>
    /// Reference for Regular Expression creation https://regexr.com
    /// </summary>
    public static class RegularExpressionsConstants
    {
        /// (                  # Start of group
        ///    (?=.*[A-Z])     #   Verify at least one uppercase character.
        ///    (?=.*[a-z])     #   Verify at least one lowercase character.
        ///    (?=.*\W)        #   Verify at least one special symbol.
        ///    (?=.*\d)        #   Verify at least one digit.
        ///       .            #     Verify anything with previous condition checking.
        ///         {9,}       #        Verify Length 9 characters or more.
        /// )
        public const string PASSWORD_FORMAT = @"((?=.*[A-Z])(?=.*[a-z])(?=.*\W)(?=.*\d).{9,}$)";
    }
}