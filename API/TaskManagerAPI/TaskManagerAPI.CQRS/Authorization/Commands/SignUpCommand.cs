using FluentResults;
using MediatR;

namespace TaskManagerAPI.CQRS.Authorization.Commands
{
    public class SignUpCommand : IRequest<Result>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}