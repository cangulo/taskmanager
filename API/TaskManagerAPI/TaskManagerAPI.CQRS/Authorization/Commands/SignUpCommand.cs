using FluentResults;
using MediatR;
using TaskManagerAPI.Models.FE;

namespace TaskManagerAPI.CQRS.Authorization.Commands
{
    public class SignUpCommand : IRequest<Result>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}