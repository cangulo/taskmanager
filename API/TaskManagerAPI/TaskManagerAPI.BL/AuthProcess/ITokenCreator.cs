using TaskManagerAPI.Models.BE;

namespace TaskManagerAPI.BL.AuthProcess
{
    public interface ITokenCreator
    {
        string CreateToken(Account account);
    }
}
