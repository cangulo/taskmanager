using FluentAssertions;
using FluentResults;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using TaskManagerAPI.BL.CurrentUserService;
using TaskManagerAPI.CQRS.TasksCQ.Queries;
using TaskManagerAPI.CQRS.TasksCQ.QueryHandlers;
using TaskManagerAPI.CQRS.Test.Contants;
using TaskManagerAPI.Models.BE.Tasks;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Repositories.TaskRepository;
using TaskManagerAPI.Resources.Errors;
using Xunit;

namespace TaskManagerAPI.CQRS.Test.TaskCQ.QueryHandlers
{
    public class TaskQueryHandlerTest
    {
        private readonly Mock<ITasksByAccountRepository> _tasksRepoByAccountMock = new Mock<ITasksByAccountRepository>();
        private readonly Mock<ICurrentUserService> _currentUserServiceMock = new Mock<ICurrentUserService>();
        private TaskQueryHandler _taskQueryHandler;

        public TaskQueryHandlerTest()
        {
            _currentUserServiceMock.Setup(a => a.GetIdCurrentUser()).Returns(Results.Ok<int>(ConstantsTasksTest.taskDomain.AccountId));
            _taskQueryHandler = new TaskQueryHandler(_tasksRepoByAccountMock.Object, _currentUserServiceMock.Object);
        }

        [Fact]
        public async Task Handle_HappyFlow()
        {
            // Arrange
            TaskQuery request = new TaskQuery
            {
                Id = ConstantsTasksTest.Id
            };
            TaskDomain tasktoReturn = ConstantsTasksTest.taskDomain;
            _tasksRepoByAccountMock.Setup(x => x.TaskExists(ConstantsTasksTest.taskDomain.AccountId, ConstantsTasksTest.Id)).Returns(true);
            _tasksRepoByAccountMock.Setup(x => x.GetTask(ConstantsTasksTest.taskDomain.AccountId, ConstantsTasksTest.Id)).Returns(tasktoReturn);

            // Act
            Result<TaskDomain> result = await _taskQueryHandler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(tasktoReturn);
        }

        [Fact]
        public async Task Handle_TaskDoesntExists_ReturnSpecificError()
        {
            // Arrange
            TaskQuery request = new TaskQuery
            {
                Id = ConstantsTasksTest.Id
            };
            _tasksRepoByAccountMock.Setup(x => x.TaskExists(ConstantsTasksTest.taskDomain.AccountId, ConstantsTasksTest.Id)).Returns(false);

            // Act
            Result<TaskDomain> result = await _taskQueryHandler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Count.Should().Be(1);
            var expectedError = new CustomError(ErrorsCodesContants.TASK_ID_NOT_FOUND, ErrorsMessagesConstants.TASK_ID_NOT_FOUND, 404);
            result.Errors[0].Should().BeEquivalentTo(expectedError);
        }
    }
}
