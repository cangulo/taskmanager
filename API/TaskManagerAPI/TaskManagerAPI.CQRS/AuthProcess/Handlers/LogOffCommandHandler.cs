using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using MediatR;
using TaskManagerAPI.BL.CurrentUserService;
using TaskManagerAPI.CQRS.AuthProcess.Commands;
using TaskManagerAPI.Models.BE;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Repositories.AccountRepository;
using TaskManagerAPI.Resources.Errors;

namespace TaskManagerAPI.CQRS.AuthProcess.Handlers
{
    public class LogOffCommandHandler : IRequestHandler<LogOffCommand, Result>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICurrentUserService _currentUserService;

        public LogOffCommandHandler(IAccountRepository accountRepository, ICurrentUserService currentUserService)
        {
            _accountRepository = accountRepository;
            _currentUserService = currentUserService;
        }

        public Task<Result> Handle(LogOffCommand request, CancellationToken cancellationToken)
        {

            Result<int> opGetUserIdResult = _currentUserService.GetIdCurrentUser();
            if (opGetUserIdResult.IsSuccess)
            {
                int userId = opGetUserIdResult.Value;
                if (this._accountRepository.ExistsAccount(userId))
                {
                    Account accountDB = this._accountRepository.GetAccount(userId);
                    accountDB.Token = null;
                    return Task.FromResult(this._accountRepository.SaveModifications());
                }
                else
                {
                    return Task.FromResult(Results.Fail(
                        new ErrorCodeAndMessage(
                            ErrorsCodesContants.USER_ID_NOT_FOUND,
                            ErrorsMessagesConstants.USER_ID_NOT_FOUND)));
                }
            }
            else
            {
                return Task.FromResult(opGetUserIdResult.ToResult());
            }
        }
    }
}