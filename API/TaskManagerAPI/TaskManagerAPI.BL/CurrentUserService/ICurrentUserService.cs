using FluentResults;

namespace TaskManagerAPI.BL.CurrentUserService
{
    /// <summary>
    /// Class to provide data of the user logged (current user)
    /// </summary>
    public interface ICurrentUserService
    {
        Result<int> GetIdCurrentUser();
    }
}