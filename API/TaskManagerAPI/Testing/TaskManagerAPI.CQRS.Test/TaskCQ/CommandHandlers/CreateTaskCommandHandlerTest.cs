using FluentAssertions;
using FluentResults;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using TaskManagerAPI.BL.CurrentUserService;
using TaskManagerAPI.CQRS.TasksCQ.CommandHandlers;
using TaskManagerAPI.CQRS.TasksCQ.Commands;
using TaskManagerAPI.Repositories.TaskRepository;
using Xunit;

namespace TaskManagerAPI.CQRS.Test.TaskCQ.CommandHandlers
{
    public class CreateTaskCommandHandlerTest
    {
        private readonly Mock<ITasksByAccountRepository> _tasksRepoByAccountMock = new Mock<ITasksByAccountRepository>();
        private readonly Mock<ICurrentUserService> _currentUserService = new Mock<ICurrentUserService>();
        private CreateTaskCommandHandler _handler;

        public CreateTaskCommandHandlerTest()
        {
            _currentUserService.Setup(X => X.GetIdCurrentUser()).Returns(Results.Ok<int>(ConstantsCQTest.Id));
            _handler = new CreateTaskCommandHandler(_tasksRepoByAccountMock.Object, _currentUserService.Object);
        }

        [Fact]
        public async Task Handle_CorrectUserCreationg_HappyFlow()
        {

            // Arrange
            CreateTaskCommand request = new CreateTaskCommand
            {
                Task = new Models.BE.Tasks.TaskDomain()
            };
            _tasksRepoByAccountMock.Setup(X => X.CreateTask(ConstantsCQTest.Id, request.Task)).Returns(Results.Ok());
            Result succesSaveModifications = Results.Ok();
            _tasksRepoByAccountMock.Setup(x => x.SaveModifications()).Returns(succesSaveModifications);

            // Act
            Result result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Should().Be(succesSaveModifications);
        }
    }
}
