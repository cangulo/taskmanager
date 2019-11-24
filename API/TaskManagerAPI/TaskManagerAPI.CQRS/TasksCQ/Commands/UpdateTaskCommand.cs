using FluentResults;
using MediatR;
using TaskManagerAPI.Models.BE.Tasks;

namespace TaskManagerAPI.CQRS.TasksCQ.Commands
{
    public class UpdateTaskCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public TaskForUpdated Task { get; set; }
    }
}