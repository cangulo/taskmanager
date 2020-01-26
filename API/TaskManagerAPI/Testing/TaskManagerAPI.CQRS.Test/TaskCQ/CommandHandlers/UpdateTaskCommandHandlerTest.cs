using AutoMapper;
using FluentAssertions;
using FluentResults;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using TaskManagerAPI.BL.CurrentUserService;
using TaskManagerAPI.CQRS.TasksCQ.CommandHandlers;
using TaskManagerAPI.CQRS.TasksCQ.Commands;
using TaskManagerAPI.CQRS.Test.Contants;
using TaskManagerAPI.Models.BE.Tasks;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Repositories.TaskRepository;
using TaskManagerAPI.Resources.Errors;
using Xunit;

namespace TaskManagerAPI.CQRS.Test.TaskCQ.CommandHandlers
{
    public class UpdateTaskCommandHandlerTest
    {
        private readonly Mock<ITasksByAccountRepository> _tasksRepoByAccountMock = new Mock<ITasksByAccountRepository>();
        private readonly Mock<ICurrentUserService> _currentUserServiceMock = new Mock<ICurrentUserService>();
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
        private UpdateTaskCommandHandler _handler;

        private readonly UpdateTaskCommand _request = new UpdateTaskCommand
        {
            Id = 1,
            Task = new Models.BE.Tasks.TaskForUpdated()
        };

        public UpdateTaskCommandHandlerTest()
        {
            _currentUserServiceMock.Setup(X => X.GetIdCurrentUser()).Returns(Results.Ok<int>(ConstantsAccountsCQTest.Id));
            _handler = new UpdateTaskCommandHandler(_tasksRepoByAccountMock.Object, _mapperMock.Object, _currentUserServiceMock.Object);
        }

        [Fact]
        public async Task Handle_UpdateExistingTask_HappyFlow()
        {
            // Arrange
            _tasksRepoByAccountMock.Setup(x => x.TaskExists(ConstantsAccountsCQTest.Id, _request.Id)).Returns(true);
            TaskDomain taskInDBMock = new TaskDomain();
            _tasksRepoByAccountMock.Setup(X => X.GetTask(ConstantsAccountsCQTest.Id, _request.Id)).Returns(taskInDBMock);
            _mapperMock.Setup(x => x.Map(_request.Task, taskInDBMock));

            Result succesSaveModifications = Results.Ok();
            _tasksRepoByAccountMock.Setup(x => x.SaveModifications()).Returns(succesSaveModifications);

            // Act
            Result result = await _handler.Handle(_request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Should().Be(succesSaveModifications);
        }


        [Fact]
        public async Task Handle_TaskDoesntExist_ReturningRepoError()
        {
            // Arrange
            _tasksRepoByAccountMock.Setup(x => x.TaskExists(ConstantsAccountsCQTest.Id, _request.Id)).Returns(false);

            // Act
            Result result = await _handler.Handle(_request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Count.Should().Be(1);
            result.Errors[0].Message.Should().Be(ErrorsMessagesConstants.TASK_ID_NOT_FOUND);
            result.Errors[0].Metadata[ErrorKeyPropsConstants.ERROR_CODE].Should().Be(ErrorsCodesContants.TASK_ID_NOT_FOUND);
            result.Errors[0].Metadata[ErrorKeyPropsConstants.ERROR_HTTP_CODE].Should().Be(404);
        }
    }
}