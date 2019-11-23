using FluentResults;
using MediatR;

namespace TaskManagerAPI.CQRS.AuthProcess.Commands
{
    public class LogOffCommand : IRequest<Result> {}
}