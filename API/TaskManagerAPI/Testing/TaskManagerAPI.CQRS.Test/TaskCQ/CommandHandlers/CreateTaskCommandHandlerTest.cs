using FluentAssertions;
using FluentResults;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using TaskManagerAPI.BL.CurrentUserService;
using TaskManagerAPI.CQRS.TasksCQ.CommandHandlers;
using TaskManagerAPI.CQRS.TasksCQ.Commands;
using TaskManagerAPI.CQRS.Test.Contants;
using TaskManagerAPI.Repositories.TaskRepository;
using Xunit;

namespace TaskManagerAPI.CQRS.Test.TaskCQ.CommandHandlers
{
    // TODO: Rename attribute _currentUserService to _currentUserServiceMock
    public class CreateTaskCommandHandlerTest
    {
        private readonly Mock<ITasksByAccountRepository> _tasksRepoByAccountMock = new Mock<ITasksByAccountRepository>();
        private readonly Mock<ICurrentUserService> _currentUserService = new Mock<ICurrentUserService>();
        private CreateTaskCommandHandler _handler;

        private readonly CreateTaskCommand _request = new CreateTaskCommand
        {
            Task = new Models.BE.Tasks.TaskDomain()
        };

        public CreateTaskCommandHandlerTest()
        {
            _currentUserService.Setup(X => X.GetIdCurrentUser()).Returns(Results.Ok<int>(ConstantsAccountsCQTest.Id));
            _handler = new CreateTaskCommandHandler(_tasksRepoByAccountMock.Object, _currentUserService.Object);
        }

        [Fact]
        public async Task Handle_CorrectUserCreationg_HappyFlow()
        {
            // Arrange

            _tasksRepoByAccountMock.Setup(X => X.CreateTask(ConstantsAccountsCQTest.Id, _request.Task)).Returns(Results.Ok());
            Result succesSaveModifications = Results.Ok();
            _tasksRepoByAccountMock.Setup(x => x.SaveModifications()).Returns(succesSaveModifications);

            // Act
            Result result = await _handler.Handle(_request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Should().Be(succesSaveModifications);
        }


        [Fact]
        public async Task Handle_ErrorCreatingTask_ReturningRepoError()
        {
            // Arrange
            string errorMsg = string.Empty;
            Result failedResult = Results.Fail(new Error(errorMsg));
            _tasksRepoByAccountMock.Setup(x => x.CreateTask(ConstantsAccountsCQTest.Id, _request.Task)).Returns(failedResult);
            
            // Act
            Result result = await _handler.Handle(_request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Should().Be(failedResult);
        }
    }
}