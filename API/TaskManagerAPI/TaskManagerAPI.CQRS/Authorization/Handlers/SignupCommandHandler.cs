using FluentResults;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskManagerAPI.CQRS.Authorization.Commands;
using TaskManagerAPI.Models.BE;
using TaskManagerAPI.Repositories.AccountRepository;

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
    }
}