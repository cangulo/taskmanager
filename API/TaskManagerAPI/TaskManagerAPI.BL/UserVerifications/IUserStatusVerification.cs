using FluentResults;

namespace TaskManagerAPI.BL.UserStatusVerification
{
    /// <summary>
    /// Service class to validate if the user is active (not blocked neither disabled).
    /// Used in <see cref="TaskManagerAPI.BL.AuthProcess.AuthService"> AuthService </see> in verify a user could log in
    /// Used in <see cref="TaskManagerAPI.Filters.AuthenticationFilterAttribute"> AuthenticationFilterAttribute </see> in verify a user could log in
    /// </summary>
    public interface IUserStatusVerification
    {
        Result<bool> UserIsActive(int userId);
    }
}