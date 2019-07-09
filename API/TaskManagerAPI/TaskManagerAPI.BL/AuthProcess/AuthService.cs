using System;
using System.Linq;
using TaskManagerAPI.EF.Context;
using TaskManagerAPI.Models.BE;
using TaskManagerAPI.Models.FE;

namespace TaskManagerAPI.BL.AuthProcess
{
    public class AuthService : IAuthService
    {
        private readonly ITaskManagerDbContext _taskManagerDbContext;
        private readonly ITokenCreator _tokenCreator;

        public AuthService(ITaskManagerDbContext taskManagerDbContext, ITokenCreator tokenCreator)
        {
            _taskManagerDbContext = taskManagerDbContext;
            _tokenCreator = tokenCreator;
        }

        public void LogOff(int userId)
        {
            if (this._taskManagerDbContext.Accounts.Any(a => a.Id == userId))
            {
                Account accountDB = this._taskManagerDbContext.Accounts.First(a => a.Id == userId);
                accountDB.Token = null;
                this._taskManagerDbContext.SaveChanges();
            }
        }

        public PortalAccount ValidateUser(string email, string password)
        {
            if (this._taskManagerDbContext.Accounts.Any(a => a.Email.Trim().ToLower() == email && a.Password.Trim().ToLower() == password))
            {
                Account accountDB = this._taskManagerDbContext.Accounts.First(a => a.Email.Trim().ToLower() == email && a.Password.Trim().ToLower() == password);

                string token = _tokenCreator.CreateToken(accountDB);
                accountDB.Token = token;
                accountDB.LastLogintime = DateTime.UtcNow;
                this._taskManagerDbContext.SaveChanges();

                return new PortalAccount()
                {
                    Username = accountDB.Username,
                    Token = token
                };
            }
            return null;
        }
    }
}
