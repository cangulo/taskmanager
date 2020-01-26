using FluentAssertions;
using FluentResults;
using Moq;
using System;
using TaskManagerAPI.BL.CurrentUserService;
using TaskManagerAPI.CQRS.TasksCQ.BaseClasses;
using TaskManagerAPI.CQRS.Test.Contants;
using Xunit;

namespace TaskManagerAPI.CQRS.Test.TaskCQ.BaseClasses
{
    public class BaseCommandQueryImplementationTest : BaseCommandQuery
    {
        public BaseCommandQueryImplementationTest(ICurrentUserService currentUserService) : base(currentUserService)
        {
        }
    }
    public class BaseCommandQueryTest
    {
        private Mock<ICurrentUserService> _currentUserServiceMock = new Mock<ICurrentUserService>();
        private BaseCommandQueryImplementationTest _baseCommandQueryImplementation;

        [Fact]
        public void BaseCommandQuery_HappyFlow_ObjectInstanciated()
        {
            //Arrange
            _currentUserServiceMock.Setup(x => x.GetIdCurrentUser()).Returns(Results.Ok<int>(ConstantsAccountsCQTest.Id));

            // Act
            _baseCommandQueryImplementation = new BaseCommandQueryImplementationTest(_currentUserServiceMock.Object);

            // Assert
            _baseCommandQueryImplementation.GetCurrentUserId().Should().Be(ConstantsAccountsCQTest.Id);
        }

        [Fact]
        public void BaseCommandQuery_ServiceInjectedNull_ThrowException()
        {
            //Arrange
            ICurrentUserService currentUserService = null;

            // Act 
            Action execConstructor = () => new BaseCommandQueryImplementationTest(currentUserService);

            // Assert
            var expectedException = new ArgumentNullException(nameof(currentUserService));
            ArgumentNullException exceptionThrow = execConstructor.Should().Throw<ArgumentNullException>().Which;
            exceptionThrow.Message.Should().BeEquivalentTo(expectedException.Message);
            exceptionThrow.ParamName.Should().BeEquivalentTo(expectedException.ParamName);
        }

        [Fact]
        public void BaseCommandQuery_GetCurrentUserIdNull_ThrowException()
        {
            //Arrange
            _currentUserServiceMock.Setup(a => a.GetIdCurrentUser()).Returns(Results.Fail(new Error()));

            // Act 
            Action execConstructor = () => new BaseCommandQueryImplementationTest(_currentUserServiceMock.Object);

            // Assert
            Result<int> opGetId = null;
            var expectedException = new ArgumentNullException(nameof(opGetId));
            ArgumentNullException exceptionThrow = execConstructor.Should().Throw<ArgumentNullException>().Which;
            exceptionThrow.Message.Should().BeEquivalentTo(expectedException.Message);
            exceptionThrow.ParamName.Should().BeEquivalentTo(expectedException.ParamName);
        }
    }
}
