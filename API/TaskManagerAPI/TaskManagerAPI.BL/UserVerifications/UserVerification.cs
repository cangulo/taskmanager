using System.Linq;
using TaskManagerAPI.EF.Context;

namespace TaskManagerAPI.BL.UserVerifications
{
    public class UserVerification : IUserVerification
    {
        private readonly ITaskManagerDbContext _taskManagerDbContext;

        public UserVerification(ITaskManagerDbContext taskManagerDbContext)
        {
            _taskManagerDbContext = taskManagerDbContext;
        }

        public bool UserIsActive(int userId)
        {
            if (this._taskManagerDbContext.Accounts.Any(x => x.Id == userId))
            {
                return this._taskManagerDbContext.Accounts.First(x => x.Id == userId).Status == Models.BE.UserStatus.Active;
            }
            else
            {
                throw new System.Exception("User Not Found");
            }
        }
    }
}
