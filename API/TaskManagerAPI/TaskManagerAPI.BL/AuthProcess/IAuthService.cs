using FluentResults;
using TaskManagerAPI.Models.FE;

namespace TaskManagerAPI.BL.AuthProcess
{
    /// <summary>
    /// Service class for authentication actions. 
    /// </summary>
    public interface IAuthService
    {
        Result LogOff();

        /// <summary>
        /// This method, following SRP, could be migrated to other service class because is the less related with authenticate actions
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Result SignUpUser(string username, string email, string password);

        Result<PortalAccount> ValidateUser(string email, string password);
    }
}
