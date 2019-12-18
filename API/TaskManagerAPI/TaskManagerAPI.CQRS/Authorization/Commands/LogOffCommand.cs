using FluentResults;
using MediatR;

namespace TaskManagerAPI.CQRS.Authorization.Commands
{
    public class LogOffCommand : IRequest<Result> { }
}