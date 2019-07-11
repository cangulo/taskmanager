namespace TaskManagerAPI.BL.UserVerifications
{
    public interface IUserVerification
    {
        bool UserIsActive(int userId);
    }
}
