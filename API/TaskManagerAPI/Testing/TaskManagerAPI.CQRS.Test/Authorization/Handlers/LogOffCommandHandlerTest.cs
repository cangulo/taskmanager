using FluentAssertions;
using FluentResults;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using TaskManagerAPI.BL.CurrentUserService;
using TaskManagerAPI.CQRS.Authorization.Commands;
using TaskManagerAPI.CQRS.Authorization.Handlers;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Repositories.AccountRepository;
using TaskManagerAPI.Resources.Errors;
using Xunit;

namespace TaskManagerAPI.CQRS.Test.Authorization.Handlers
{
    public class LogOffCommandHandlerTest
    {
        private readonly Mock<IAccountRepository> _accountRepository = new Mock<IAccountRepository>();
        private readonly Mock<ICurrentUserService> _currentUserService = new Mock<ICurrentUserService>();
        private LogOffCommandHandler _handler;

        public LogOffCommandHandlerTest()
        {
            _handler = new LogOffCommandHandler(_accountRepository.Object, _currentUserService.Object);
        }

        [Fact]
        public async Task Handle_ValidUser_HappyFlow()
        {
            // Arrange

            LogOffCommand request = new LogOffCommand();
            int accountId = ConstantsAccountsCQTest.Id;
            _currentUserService.Setup(x => x.GetIdCurrentUser()).Returns(Results.Ok<int>(accountId));
            _accountRepository.Setup(x => x.ExistsAccount(accountId)).Returns(true);
            _accountRepository.Setup(x => x.GetAccount(accountId)).Returns(ConstantsAccountsCQTest.AccountTest);
            Result successResult = Results.Ok();
            _accountRepository.Setup(x => x.SaveModifications()).Returns(successResult);

            // Act

            Result result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Should().Be(successResult);
            ConstantsAccountsCQTest.AccountTest.Token.Should().Be(string.Empty);
        }

        [Fact]
        public async Task Handle_ErrorGettingCurrentUser_ReturnGetCurrentUserError()
        {
            // Arrange

            LogOffCommand request = new LogOffCommand();
            Result failedResult = Results.Fail<int>(new Error());
            _currentUserService.Setup(x => x.GetIdCurrentUser()).Returns(failedResult);

            // Act

            Result result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(failedResult);
        }

        [Fact]
        public async Task Handle_CurrentUserDoesntExist_SpecificErrorAnd401()
        {
            // Arrange

            LogOffCommand request = new LogOffCommand();
            int accountId = ConstantsAccountsCQTest.Id;
            _currentUserService.Setup(x => x.GetIdCurrentUser()).Returns(Results.Ok<int>(accountId));
            _accountRepository.Setup(x => x.ExistsAccount(accountId)).Returns(false);

            // Act

            Result result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Count.Should().Be(1);
            result.Errors[0].Message.Should().Be(ErrorsMessagesConstants.USER_ID_NOT_FOUND);
            result.Errors[0].Metadata[ErrorKeyPropsConstants.ERROR_CODE].Should().Be(ErrorsCodesContants.USER_ID_NOT_FOUND);
            result.Errors[0].Metadata[ErrorKeyPropsConstants.ERROR_HTTP_CODE].Should().Be(401);
        }
    }
}