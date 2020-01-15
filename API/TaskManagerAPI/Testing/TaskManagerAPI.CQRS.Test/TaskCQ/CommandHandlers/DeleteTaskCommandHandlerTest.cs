using FluentResults;
using Moq;
using System.Threading.Tasks;
using System.Threading;
using TaskManagerAPI.BL.CurrentUserService;
using TaskManagerAPI.CQRS.TasksCQ.CommandHandlers;
using TaskManagerAPI.CQRS.TasksCQ.Commands;
using TaskManagerAPI.Repositories.TaskRepository;
using Xunit;
using FluentAssertions;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Resources.Errors;

namespace TaskManagerAPI.CQRS.Test.TaskCQ.CommandHandlers
{
    public class DeleteTaskCommandHandlerTest
    {
        private readonly Mock<ITasksByAccountRepository> _tasksRepoByAccountMock = new Mock<ITasksByAccountRepository>();
        private readonly Mock<ICurrentUserService> _currentUserService = new Mock<ICurrentUserService>();
        private DeleteTaskCommandHandler _handler;

        private readonly DeleteTaskCommand _request = new DeleteTaskCommand
        {
            Id = 1
        };

        public DeleteTaskCommandHandlerTest()
        {
            _currentUserService.Setup(X => X.GetIdCurrentUser()).Returns(Results.Ok<int>(ConstantsCQTest.Id));
            _handler = new DeleteTaskCommandHandler(_tasksRepoByAccountMock.Object, _currentUserService.Object);
        }

        [Fact]
        public async Task Handle_DeleteExistingTask_HappyFlow()
        {
            // Arrange
            _tasksRepoByAccountMock.Setup(x => x.TaskExists(ConstantsCQTest.Id, _request.Id)).Returns(true);
            _tasksRepoByAccountMock.Setup(X => X.DeleteTask(ConstantsCQTest.Id, _request.Id));

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
            _tasksRepoByAccountMock.Setup(x => x.TaskExists(ConstantsCQTest.Id, _request.Id)).Returns(false);

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