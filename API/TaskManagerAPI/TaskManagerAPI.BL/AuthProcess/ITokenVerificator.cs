namespace TaskManagerAPI.BL.AuthProcess
{
    public interface ITokenVerificator
    {
        bool TokenIsValid(int userId, string token);
    }
}
