using TaskManagerAPI.Models.FE;

namespace TaskManagerAPI.BL.AuthProcess
{
    public interface IAuthService
    {
        void LogOff(int userId);
        PortalAccount ValidateUser(string email, string password);
    }
}
