using FluentResults;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskManagerAPI.BL.AuthProcess;
using TaskManagerAPI.BL.UserStatusVerification;
using TaskManagerAPI.CQRS.Authorization.Commands;
using TaskManagerAPI.Models.BE;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Models.FE;
using TaskManagerAPI.Repositories.AccountRepository;
using TaskManagerAPI.Resources.Errors;

namespace TaskManagerAPI.CQRS.Authorization.Handlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<PortalAccount>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITokenCreator _tokenCreator;
        private readonly IUserStatusVerification _userStatusVerification;

        public LoginCommandHandler(IAccountRepository accountRepository, ITokenCreator tokenCreator, IUserStatusVerification userStatusVerification)
        {
            _accountRepository = accountRepository;
            _tokenCreator = tokenCreator;
            _userStatusVerification = userStatusVerification;
        }

        public Task<Result<PortalAccount>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            if (_accountRepository.ExistsAccount(request.Email, request.Password))
            {
                Account accountDB = _accountRepository.GetAccount(request.Email, request.Password);

                Result userIsActiveQuery = _userStatusVerification.UserIsActive(accountDB.Id);

                if (userIsActiveQuery.IsSuccess)
                {
                    string token = _tokenCreator.CreateToken(accountDB);
                    accountDB.Token = token;
                    accountDB.LastLogintime = DateTime.UtcNow;

                    Result saveResult = _accountRepository.SaveModifications();
                    if (saveResult.IsSuccess)
                    {
                        return Task.FromResult(
                            Results.Ok(new PortalAccount()
                            {
                                Username = accountDB.Username,
                                Token = token
                            }));
                    }
                    else
                    {
                        return Task.FromResult(saveResult.ToResult<PortalAccount>());
                    }
                }
                else
                {
                    return Task.FromResult(userIsActiveQuery.ToResult<PortalAccount>());
                }
            }
            return Task.FromResult(Results.Fail<PortalAccount>(
                    new CustomError(ErrorsCodesContants.INVALID_EMAIL_OR_PASSWORD, ErrorsMessagesConstants.INVALID_EMAIL_OR_PASSWORD, 401)));
        }
    }
}