using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using MediatR;
using TaskManagerAPI.CQRS.Authorization.Commands;
using TaskManagerAPI.Models.BE;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Repositories.AccountRepository;
using TaskManagerAPI.Resources.Errors;

namespace TaskManagerAPI.CQRS.Authorization.Handlers
{
    public class SignupCommandHandler : IRequestHandler<SignUpCommand, Result>
    {
        private readonly IAccountRepository _accountRepository;

        public SignupCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public Task<Result> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            if (!this._accountRepository.ExistsAccount(request.Email))
            {
                Account newAccount = new Account()
                {
                    Email = request.Email.ToLower(),
                    Username = request.FullName,
                    Password = request.Password

                };
                this._accountRepository.CreateAccount(newAccount);

                Result saveResult = this._accountRepository.SaveModifications();
                if (saveResult.IsSuccess)
                {
                    return Task.FromResult(Results.Ok());
                }
                else
                {
                    return Task.FromResult(saveResult);
                }
            }
            else
            {
                return Task.FromResult(Results.Fail(
                    new CustomError(ErrorsCodesContants.EMAIL_ALREADY_USED, ErrorsMessagesConstants.EMAIL_ALREADY_USED, 400)));
            }
        }
    }
}