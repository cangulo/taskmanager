using FluentResults;
using MediatR;
using System.Collections.Generic;
using TaskManagerAPI.Models.BE.Tasks;

namespace TaskManagerAPI.CQRS.TasksCQ.Queries
{
    public class TaskCollectionQuery : IRequest<Result<IReadOnlyCollection<TaskDomain>>>
    {
    }
}