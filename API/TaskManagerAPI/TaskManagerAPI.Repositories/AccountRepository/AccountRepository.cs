using FluentResults;
using System.Linq;
using TaskManagerAPI.EF.Context;
using TaskManagerAPI.Models.BE;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Resources.Errors;

namespace TaskManagerAPI.Repositories.AccountRepository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ITaskManagerDbContext _dbContext;

        public AccountRepository(ITaskManagerDbContext taskManagerDbContext)
        {
            _dbContext = taskManagerDbContext;
        }

        public bool ExistsAccount(int id)
        {
            return this._dbContext.Accounts.Any(a => a.Id == id);
        }

        public Account GetAccount(int id)
        {
            return this._dbContext.Accounts.FirstOrDefault(a => a.Id == id);
        }

        public bool ExistsAccount(string email, string password)
        {
            return this._dbContext.Accounts.Any(a => a.Email.Trim().ToLower() == email && a.Password == password);
        }

        public bool ExistsAccount(string email)
        {
            return this._dbContext.Accounts.Any(a => a.Email.Trim().ToLower() == email);
        }

        public Account GetAccount(string email, string password)
        {
            return this._dbContext.Accounts.FirstOrDefault(a => a.Email.Trim().ToLower() == email && a.Password == password);
        }

        public void CreateAccount(Account account)
        {
            _dbContext.Accounts.Add(account);
        }

        public Result SaveModifications()
        {
            if (_dbContext.SaveChanges() >= 0)
            {
                return Results.Ok();
            }
            else
            {
                return Results.Fail(new CustomError(
                    ErrorsCodesContants.UNABLE_TO_SAVE_CHANGES_IN_ACCOUNT_TABLE,
                    ErrorsMessagesConstants.UNABLE_TO_SAVE_CHANGES_IN_ACCOUNT_TABLE, 500));
            }
        }
    }
}