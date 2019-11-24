using FluentResults;
using MediatR;
using TaskManagerAPI.Models.BE.Tasks;

namespace TaskManagerAPI.CQRS.TasksCQ.Queries
{
    public class TaskQuery : IRequest<Result<Task>>
    {
        public int Id { get; set; }
    }
}
