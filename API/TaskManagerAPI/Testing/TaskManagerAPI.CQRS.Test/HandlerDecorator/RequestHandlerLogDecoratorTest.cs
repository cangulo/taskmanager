using FluentAssertions;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TaskManagerAPI.CQRS.Exceptions;
using TaskManagerAPI.CQRS.HandlerDecorator;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Test.Common.LoggerExtensions;
using Xunit;
using Xunit.Abstractions;

namespace TaskManagerAPI.CQRS.Test.HandlerDecorator
{

    public class RequestHandlerLogDecoratorTest
    {
        private readonly Mock<IRequestHandler<RequestTestClass, ResultBase>> _decorated = new Mock<IRequestHandler<RequestTestClass, ResultBase>>();
        private Mock<ILogger<RequestHandlerLogDecorator<RequestTestClass, ResultBase>>> _logger = new Mock<ILogger<RequestHandlerLogDecorator<RequestTestClass, ResultBase>>>();
        private RequestHandlerLogDecorator<RequestTestClass, ResultBase> _handlerDecorator;
        public RequestHandlerLogDecoratorTest(ITestOutputHelper testOutputHelper)
        {
            _logger.RedirectLogOutputToTestOutput(testOutputHelper);
            _handlerDecorator = new RequestHandlerLogDecorator<RequestTestClass, ResultBase>(_decorated.Object, _logger.Object);
        }

        [Fact]
        public async Task Handle_HappyFlow_LogTimeRequired()
        {
            // Arrange
            var request = new RequestTestClass();
            var expectedResult = Results.Ok();
            _decorated
                .Setup(x => x
                    .Handle(request, CancellationToken.None))
                .Returns(Task.FromResult(expectedResult as ResultBase));

            // Act
            ResultBase result = await _handlerDecorator
                                        .Handle(request, CancellationToken.None);

            // Assert
            result.Should().Be(expectedResult);
            var _regexVerifyInfoMsg = new Regex(@"(^Handler;\sIRequestHandler`2Proxy\s;\sTimeRequired:\s\d{1,4}ms)$");
            _logger.VerifyLog(LogLevel.Information, _regexVerifyInfoMsg, Times.Once());
        }

        [Fact]
        public void Handle_Error_ThrowException()
        {
            // Arrange
            var request = new RequestTestClass();
            var error = new CustomError("errorCodeTest", "errorMessageTest", 500);
            var errorList = new List<CustomError> { error };
            var expectedResult = Results.Fail(error);

            _decorated
                .Setup(x => x
                    .Handle(request, CancellationToken.None))
                .Returns(Task.FromResult(expectedResult as ResultBase));

            // Act
            Func<Task> act = async () => await _handlerDecorator
                                        .Handle(request, CancellationToken.None);

            // Assert
            var exceptionResult = act.Should()
                .Throw<CQException>()
                .Which;

            exceptionResult
                .Errors()
                .Should()
                .BeEquivalentTo(errorList);

            var regexVerifyTimeElapsedErrorMsg = new Regex(@"(^Handler;\sIRequestHandler`2Proxy\s;\s\d{1,4}ms)$");
            _logger.VerifyLog(LogLevel.Error, regexVerifyTimeElapsedErrorMsg, Times.Once());

            var errorStringMsg = string.Join(",", errorList.Select(er => er.ToString()));
            var regexVerifyErrorMsg = new Regex($"(^Handler;\\sIRequestHandler`2Proxy\\s;\\sErrors:\\s){{1}}({errorStringMsg}){{1}}$");
            _logger.VerifyLog(LogLevel.Error, regexVerifyErrorMsg, Times.Once());
        }
    }
}