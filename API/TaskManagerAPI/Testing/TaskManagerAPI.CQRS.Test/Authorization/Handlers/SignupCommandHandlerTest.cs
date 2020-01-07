using FluentAssertions;
using FluentResults;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using TaskManagerAPI.CQRS.Authorization.Commands;
using TaskManagerAPI.CQRS.Authorization.Handlers;
using TaskManagerAPI.Models.BE;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Repositories.AccountRepository;
using TaskManagerAPI.Resources.Errors;
using Xunit;

namespace TaskManagerAPI.CQRS.Test.Authorization.Handlers
{
    public class SignupCommandHandlerTest
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock = new Mock<IAccountRepository>();
        private SignupCommandHandler _handler;

        public SignupCommandHandlerTest()
        {
            _handler = new SignupCommandHandler(this._accountRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_HappyFlow()
        {
            // Arrange
            SignUpCommand request = new SignUpCommand
            {
                FullName = ConstantsCQTest.Username,
                Email = ConstantsCQTest.Email,
                Password = ConstantsCQTest.Password
            };
            _accountRepositoryMock.Setup(x => x.ExistsAccount(request.Email)).Returns(false);
            _accountRepositoryMock.Setup(x => x.SaveModifications()).Returns(Results.Ok());

            // Act
            Result result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();

            Account newAccount = new Account()
            {
                Email = request.Email.ToLower(),
                Username = request.FullName,
                Password = request.Password
            };

            // Verify CreateAccount method receive the Account object with the properly attributes
            _accountRepositoryMock
                .Verify(x =>
                    x.CreateAccount(
                        It.Is<Account>(x =>
                            x.Password == newAccount.Password &&
                            x.Email == newAccount.Email.ToLower() &&
                            x.Username == newAccount.Username)), Times.Once);
        }
        [Fact]
        public async Task Handle_UserAlreadyExists_SpecificError()
        {
            // Arrange
            SignUpCommand request = new SignUpCommand
            {
                FullName = ConstantsCQTest.Username,
                Email = ConstantsCQTest.Email,
                Password = ConstantsCQTest.Password
            };
            _accountRepositoryMock.Setup(x => x.ExistsAccount(request.Email)).Returns(true);

            // Act
            Result result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Count.Should().Be(1);
            result.Errors[0].Message.Should().Be(ErrorsMessagesConstants.EMAIL_ALREADY_USED);
            result.Errors[0].Metadata[ErrorKeyPropsConstants.ERROR_CODE].Should().Be(ErrorsCodesContants.EMAIL_ALREADY_USED);
            result.Errors[0].Metadata[ErrorKeyPropsConstants.ERROR_HTTP_CODE].Should().Be(400);
        }

        [Fact]
        public async Task Handle_ErrorCreatingNewAccount_ReturnRepoError()
        {
            // Arrange
            SignUpCommand request = new SignUpCommand
            {
                FullName = ConstantsCQTest.Username,
                Email = ConstantsCQTest.Email,
                Password = ConstantsCQTest.Password
            };
            _accountRepositoryMock.Setup(x => x.ExistsAccount(request.Email)).Returns(false);
            Result failedResult = Results.Fail(new Error());
            _accountRepositoryMock.Setup(x => x.SaveModifications()).Returns(failedResult);

            // Act
            Result result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Should().Be(failedResult);
        }
    }
}