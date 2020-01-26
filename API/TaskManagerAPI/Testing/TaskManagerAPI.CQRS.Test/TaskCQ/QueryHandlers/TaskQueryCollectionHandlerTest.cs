using FluentAssertions;
using FluentResults;
using Moq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using TaskManagerAPI.BL.CurrentUserService;
using TaskManagerAPI.CQRS.TasksCQ.Queries;
using TaskManagerAPI.CQRS.TasksCQ.QueryHandlers;
using TaskManagerAPI.CQRS.Test.Contants;
using TaskManagerAPI.Models.BE.Tasks;
using TaskManagerAPI.Repositories.TaskRepository;
using Xunit;

namespace TaskManagerAPI.CQRS.Test.TaskCQ.QueryHandlers
{
    public class TaskQueryCollectionHandlerTest
    {
        private readonly Mock<ITasksByAccountRepository> _tasksRepoByAccountMock = new Mock<ITasksByAccountRepository>();
        private readonly Mock<ICurrentUserService> _currentUserServiceMock = new Mock<ICurrentUserService>();
        private TaskCollectionQueryHandler _taskCollectionQueryHandler;

        public TaskQueryCollectionHandlerTest()
        {
            _currentUserServiceMock.Setup(a => a.GetIdCurrentUser()).Returns(Results.Ok<int>(ConstantsTasksTest.taskDomain.AccountId));
            _taskCollectionQueryHandler = new TaskCollectionQueryHandler(_tasksRepoByAccountMock.Object, _currentUserServiceMock.Object);
        }

        [Fact]
        public async Task Handle_HappyFlow()
        {
            // Arrange
            TaskCollectionQuery request = new TaskCollectionQuery();
            ReadOnlyCollection<TaskDomain> tasksToReturn = new ReadOnlyCollection<TaskDomain>(new List<TaskDomain> { ConstantsTasksTest.taskDomain });
            _tasksRepoByAccountMock.Setup(x => x.GetTasks(ConstantsTasksTest.taskDomain.AccountId)).Returns(tasksToReturn);

            // Act
            Result<IReadOnlyCollection<TaskDomain>> result = await _taskCollectionQueryHandler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeSameAs(tasksToReturn);
        }

    }
}
