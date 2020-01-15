using FluentAssertions;
using FluentResults;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskManagerAPI.BL.AuthProcess;
using TaskManagerAPI.BL.UserStatusVerification;
using TaskManagerAPI.CQRS.Authorization.Commands;
using TaskManagerAPI.CQRS.Authorization.Handlers;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Models.FE;
using TaskManagerAPI.Repositories.AccountRepository;
using TaskManagerAPI.Resources.Errors;
using Xunit;

namespace TaskManagerAPI.CQRS.Test.Authorization.Handlers
{
    public class LoginCommandHandlerTest : IDisposable
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock = new Mock<IAccountRepository>();
        private readonly Mock<ITokenCreator> _tokenCreatorMock = new Mock<ITokenCreator>();
        private readonly Mock<IUserStatusVerification> _userStatusVerificationMock = new Mock<IUserStatusVerification>();
        private LoginCommandHandler _handler;

        public LoginCommandHandlerTest()
        {
            _handler = new LoginCommandHandler(_accountRepositoryMock.Object, _tokenCreatorMock.Object, _userStatusVerificationMock.Object);
        }

        public void Dispose()
        {
            // No need to be implemented
        }

        [Fact]
        public async Task Handle_ValidAccount_HappyFlow()
        {
            // Arrange

            LoginCommand request = new LoginCommand
            {
                Email = ConstantsAccountsCQTest.Email,
                Password = ConstantsAccountsCQTest.Password
            };
            _accountRepositoryMock.Setup(x => x.ExistsAccount(request.Email, request.Password)).Returns(true);
            _accountRepositoryMock.Setup(x => x.GetAccount(request.Email, request.Password)).Returns(ConstantsAccountsCQTest.AccountTest);
            _userStatusVerificationMock.Setup(x => x.UserIsActive(ConstantsAccountsCQTest.AccountTest.Id)).Returns(Results.Ok());
            _tokenCreatorMock.Setup(x => x.CreateToken(ConstantsAccountsCQTest.AccountTest)).Returns(ConstantsAccountsCQTest.Token2);
            _accountRepositoryMock.Setup(x => x.SaveModifications()).Returns(Results.Ok());

            // Act

            Result<PortalAccount> result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(new PortalAccount { Token = ConstantsAccountsCQTest.Token2, Username = ConstantsAccountsCQTest.AccountTest.Username });
            _accountRepositoryMock.Verify(x => x.SaveModifications(), Times.Once());
            ConstantsAccountsCQTest.AccountTest.Token.Should().Be(ConstantsAccountsCQTest.Token2);
            ConstantsAccountsCQTest.AccountTest.LastLogintime.Should().BeAfter(DateTime.MinValue);
            ConstantsAccountsCQTest.AccountTest.LastLogintime.Should().BeAfter(DateTime.UtcNow.AddMinutes(-1));
            ConstantsAccountsCQTest.AccountTest.LastLogintime.Should().BeBefore(DateTime.UtcNow);
        }

        [Fact]
        public async Task Handle_AccountDoesntExist_SpecificErrorAnd401()
        {
            // Arrange

            LoginCommand request = new LoginCommand
            {
                Email = ConstantsAccountsCQTest.Email,
                Password = ConstantsAccountsCQTest.Password
            };
            _accountRepositoryMock.Setup(x => x.ExistsAccount(request.Email, request.Password)).Returns(false);

            // Act

            Result<PortalAccount> result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Count.Should().Be(1);
            result.Errors[0].Message.Should().Be(ErrorsMessagesConstants.INVALID_EMAIL_OR_PASSWORD);
            result.Errors[0].Metadata[ErrorKeyPropsConstants.ERROR_CODE].Should().Be(ErrorsCodesContants.INVALID_EMAIL_OR_PASSWORD);
            result.Errors[0].Metadata[ErrorKeyPropsConstants.ERROR_HTTP_CODE].Should().Be(401);
        }

        [Fact]
        public async Task Handle_AccountIsNotActive_SpecificErrorAnd401()
        {
            // Arrange

            LoginCommand request = new LoginCommand
            {
                Email = ConstantsAccountsCQTest.Email,
                Password = ConstantsAccountsCQTest.Password
            };
            _accountRepositoryMock.Setup(x => x.ExistsAccount(request.Email, request.Password)).Returns(true);
            _accountRepositoryMock.Setup(x => x.GetAccount(request.Email, request.Password)).Returns(ConstantsAccountsCQTest.AccountTest);
            CustomError errorUserDisabled = new CustomError(ErrorsCodesContants.USER_DISABLED, ErrorsMessagesConstants.USER_DISABLED, 401);
            _userStatusVerificationMock.Setup(x => x.UserIsActive(ConstantsAccountsCQTest.AccountTest.Id)).Returns(Results.Fail(errorUserDisabled));

            // Act

            Result<PortalAccount> result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Count.Should().Be(1);
            result.Errors[0].Message.Should().Be(ErrorsMessagesConstants.USER_DISABLED);
            result.Errors[0].Metadata[ErrorKeyPropsConstants.ERROR_CODE].Should().Be(errorUserDisabled.Metadata[ErrorKeyPropsConstants.ERROR_CODE]);
            result.Errors[0].Metadata[ErrorKeyPropsConstants.ERROR_HTTP_CODE].Should().Be(errorUserDisabled.Metadata[ErrorKeyPropsConstants.ERROR_HTTP_CODE]);
        }

        [Fact]
        public async Task Handle_ErrorSavingChanges_SpecificError500()
        {
            // Arrange

            LoginCommand request = new LoginCommand
            {
                Email = ConstantsAccountsCQTest.Email,
                Password = ConstantsAccountsCQTest.Password
            };
            _accountRepositoryMock.Setup(x => x.ExistsAccount(request.Email, request.Password)).Returns(true);
            _accountRepositoryMock.Setup(x => x.GetAccount(request.Email, request.Password)).Returns(ConstantsAccountsCQTest.AccountTest);
            _userStatusVerificationMock.Setup(x => x.UserIsActive(ConstantsAccountsCQTest.AccountTest.Id)).Returns(Results.Ok());
            _tokenCreatorMock.Setup(x => x.CreateToken(ConstantsAccountsCQTest.AccountTest)).Returns(ConstantsAccountsCQTest.Token2);
            CustomError errorSavingModifications = new CustomError(
                    ErrorsCodesContants.UNABLE_TO_SAVE_CHANGES_IN_ACCOUNT_TABLE,
                    ErrorsMessagesConstants.UNABLE_TO_SAVE_CHANGES_IN_ACCOUNT_TABLE, 500);
            _accountRepositoryMock.Setup(x => x.SaveModifications()).Returns(Results.Fail(errorSavingModifications));

            // Act

            Result<PortalAccount> result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Count.Should().Be(1);
            result.Errors[0].Message.Should().Be(ErrorsMessagesConstants.UNABLE_TO_SAVE_CHANGES_IN_ACCOUNT_TABLE);
            result.Errors[0].Metadata[ErrorKeyPropsConstants.ERROR_CODE].Should().Be(errorSavingModifications.Metadata[ErrorKeyPropsConstants.ERROR_CODE]);
            result.Errors[0].Metadata[ErrorKeyPropsConstants.ERROR_HTTP_CODE].Should().Be(errorSavingModifications.Metadata[ErrorKeyPropsConstants.ERROR_HTTP_CODE]);
        }
    }
}