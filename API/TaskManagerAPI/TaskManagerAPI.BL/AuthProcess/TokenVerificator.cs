using System.Linq;
using TaskManagerAPI.EF.Context;

namespace TaskManagerAPI.BL.AuthProcess
{
    public class TokenVerificator : ITokenVerificator
    {
        private readonly ITaskManagerDbContext _taskManagerDbContext;

        public TokenVerificator(ITaskManagerDbContext taskManagerDbContext)
        {
            _taskManagerDbContext = taskManagerDbContext;
        }

        public bool TokenIsValid(int userId, string token)
        {
            return this._taskManagerDbContext.Accounts.First(ac => ac.Id == userId).Token == token;
        }
    }
}