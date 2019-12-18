using FluentResults;
using TaskManagerAPI.Models.BE;

namespace TaskManagerAPI.Repositories.AccountRepository
{
    /// <summary>
    /// Repository Class for CRUD operations related with the Table Accounts
    /// </summary>
    public interface IAccountRepository
    {
        bool ExistsAccount(int id);

        bool ExistsAccount(string email, string password);

        bool ExistsAccount(string email);

        Account GetAccount(int id);

        Account GetAccount(string email, string password);

        void CreateAccount(Account account);

        Result SaveModifications();
    }
}