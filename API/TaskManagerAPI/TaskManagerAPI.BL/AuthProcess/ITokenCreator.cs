using TaskManagerAPI.Models.BE;

namespace TaskManagerAPI.BL.AuthProcess
{
    /// <summary>
    /// Service class for token creation.
    /// </summary>
    public interface ITokenCreator
    {
        string CreateToken(Account account);
    }
}