using FluentAssertions;
using System.Collections.Generic;
using TaskManagerAPI.CQRS.Exceptions;
using TaskManagerAPI.Exceptions.Handlers;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Resources.Errors;
using Xunit;

namespace TaskManagerAPI.Test.Exceptions.Handlers
{
    public class CQExceptionHandlerTest
    {
        // TODO: Test null input value CQEXception
        [Fact]
        public void CQExceptionHandler_ConstructorNoError_AddDefaultError()
        {
            // Arrange
            CQException cQException = new CQException(new List<CustomError>());

            CustomError unkownError = new CustomError(ErrorsCodesContants.UNKNOWN_ERROR_API, ErrorsMessagesConstants.UNKNOWN_ERROR_API, 500);
            CQException cQExceptionWithDefaultError = new CQException(new List<CustomError> { unkownError });
            // Act
            CQExceptionHandler handlerResult = new CQExceptionHandler(cQException);
            CQExceptionHandler handlerExpected = new CQExceptionHandler(cQExceptionWithDefaultError);
            // Assert
            handlerResult.CreateResponseContent().Should().Be(handlerExpected.CreateResponseContent());
            handlerResult.GetHttpStatusCode().Should().Be(handlerExpected.GetHttpStatusCode());
        }

        [Fact]
        public void CreateResponseContent_ErrorAttached()
        {
        }

        [Fact]
        public void CreateResponseContent_DefaultErrorAttached()
        {
        }

        [Fact]
        public void GetHttpStatusCode_ErrorsAttached()
        {
            // Arrange
            CustomError customError = new CustomError(ErrorsCodesContants.INVALID_EMAIL_OR_PASSWORD, ErrorsMessagesConstants.INVALID_EMAIL_OR_PASSWORD, 401);
            CQException cQException = new CQException(new List<CustomError> { customError });
            CQExceptionHandler handler = new CQExceptionHandler(cQException);
            // Act
            int result = handler.GetHttpStatusCode();
            // Assert
            result.Should().Be(401);
        }

        [Fact]
        public void GetHttpStatusCode_DefaultErrorAttached()
        {
        }
    }
}