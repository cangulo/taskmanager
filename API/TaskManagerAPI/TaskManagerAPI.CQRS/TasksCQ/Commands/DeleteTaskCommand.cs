using FluentResults;
using MediatR;

namespace TaskManagerAPI.CQRS.TasksCQ.Commands
{
    public class DeleteTaskCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }
}