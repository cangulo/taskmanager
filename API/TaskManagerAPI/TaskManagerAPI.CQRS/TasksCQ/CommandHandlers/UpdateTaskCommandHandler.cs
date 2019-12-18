using AutoMapper;
using FluentResults;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskManagerAPI.BL.CurrentUserService;
using TaskManagerAPI.CQRS.TasksCQ.BaseClasses;
using TaskManagerAPI.CQRS.TasksCQ.Commands;
using TaskManagerAPI.Models.BE.Tasks;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Repositories.TaskRepository;
using TaskManagerAPI.Resources.Errors;

namespace TaskManagerAPI.CQRS.TasksCQ.CommandHandlers
{
    public class UpdateTaskCommandHandler : BaseCommandQuery, IRequestHandler<UpdateTaskCommand, Result>
    {
        private readonly ITasksByAccountRepository _tasksRepoByAccount;
        private readonly IMapper _mapper;

        public UpdateTaskCommandHandler(ITasksByAccountRepository tasksRepoByAccount, IMapper mapper, ICurrentUserService currentUserService) : base(currentUserService)
        {
            _tasksRepoByAccount = tasksRepoByAccount;
            _mapper = mapper;
        }

        public Task<Result> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            if (this._tasksRepoByAccount.TaskExists(this.GetCurrentUserId(), request.Id))
            {
                TaskDomain taskInDb = _tasksRepoByAccount.GetTask(this.GetCurrentUserId(), request.Id);
                _mapper.Map(request.Task, taskInDb);
                return Task.FromResult(_tasksRepoByAccount.SaveModifications());
            }
            else
            {
                return Task.FromResult(Results.Fail(
                    new CustomError(ErrorsCodesContants.TASK_ID_NOT_FOUND, ErrorsMessagesConstants.TASK_ID_NOT_FOUND, 404)));
            }
        }
    }
}