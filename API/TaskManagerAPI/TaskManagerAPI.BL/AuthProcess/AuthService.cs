using FluentResults;
using System;
using TaskManagerAPI.BL.CurrentUserService;
using TaskManagerAPI.BL.UserStatusVerification;
using TaskManagerAPI.Models.BE;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Models.FE;
using TaskManagerAPI.Repositories.AccountRepository;
using TaskManagerAPI.Resources.Errors;

namespace TaskManagerAPI.BL.AuthProcess
{
    public class AuthService : IAuthService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITokenCreator _tokenCreator;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserStatusVerification _userStatusVerification;
        public AuthService(IAccountRepository accountRepository, ITokenCreator tokenCreator, ICurrentUserService currentUserService, IUserStatusVerification userStatusVerification)
        {
            _accountRepository = accountRepository;
            _tokenCreator = tokenCreator;
            _currentUserService = currentUserService;
            _userStatusVerification = userStatusVerification;
        }

        public Result LogOff()
        {
            Result<int> opGetUserIdResult = _currentUserService.GetIdCurrentUser();
            if (opGetUserIdResult.IsSuccess)
            {
                int userId = opGetUserIdResult.Value;
                if (this._accountRepository.ExistsAccount(userId))
                {
                    Account accountDB = this._accountRepository.GetAccount(userId);
                    accountDB.Token = null;
                    return this._accountRepository.SaveModifications();
                }
                else
                {
                    return Results.Fail(
                        new ErrorCodeAndMessage(
                            ErrorsCodesContants.USER_ID_NOT_FOUND,
                            ErrorsMessagesConstants.USER_ID_NOT_FOUND));
                }
            }
            else
            {
                return opGetUserIdResult.ToResult();
            }
        }

        public Result<PortalAccount> ValidateUser(string email, string password)
        {
            if (this._accountRepository.ExistsAccount(email, password))
            {
                Account accountDB = this._accountRepository.GetAccount(email, password);

                Result<bool> userIsActiveQuery = _userStatusVerification.UserIsActive(accountDB.Id);

                if (userIsActiveQuery.IsSuccess && userIsActiveQuery.Value)
                {
                    string token = _tokenCreator.CreateToken(accountDB);
                    accountDB.Token = token;
                    accountDB.LastLogintime = DateTime.UtcNow;

                    Result saveResult = this._accountRepository.SaveModifications();
                    if (saveResult.IsSuccess)
                    {
                        return Results.Ok<PortalAccount>(new PortalAccount()
                        {
                            Username = accountDB.Username,
                            Token = token
                        });
                    }
                    else
                    {
                        return saveResult.ToResult<PortalAccount>();
                    }
                }
                else
                {
                    return userIsActiveQuery.ToResult<PortalAccount>();
                }
            }
            return Results.Fail<PortalAccount>(
                    new ErrorCodeAndMessage(
                        ErrorsCodesContants.INVALID_EMAIL_OR_PASSWORD,
                        ErrorsMessagesConstants.INVALID_EMAIL_OR_PASSWORD));
        }

        public Result SignUpUser(string username, string email, string password)
        {
            if (!this._accountRepository.ExistsAccount(email))
            {
                Account newAccount = new Account()
                {
                    Email = email.ToLower(),
                    Username = username,
                    Password = password

                };
                this._accountRepository.CreateAccount(newAccount);

                Result saveResult = this._accountRepository.SaveModifications();
                if (saveResult.IsSuccess)
                {
                    return Results.Ok();
                }
                else
                {
                    return saveResult;
                }
            }
            else
            {
                return Results.Fail(
                    new ErrorCodeAndMessage(
                        ErrorsCodesContants.EMAIL_ALREADY_USED,
                        ErrorsMessagesConstants.EMAIL_ALREADY_USED));
            }
        }
    }
}
