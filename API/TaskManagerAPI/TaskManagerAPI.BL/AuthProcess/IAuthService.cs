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
    }
}
