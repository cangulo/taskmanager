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
    }
}
