using FluentResults;
using MediatR;
using TaskManagerAPI.Models.BE.Tasks;

namespace TaskManagerAPI.CQRS.TasksCQ.Commands
{
    public class CreateTaskCommand : IRequest<Result>
    {
        public TaskDomain Task { get; set; }
    }
}