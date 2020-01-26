using FluentAssertions;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TaskManagerAPI.CQRS.HandlerDecorator;
using Xunit;

namespace TaskManagerAPI.CQRS.Test.HandlerDecorator
{
    public class RequestTestClass : IRequest<ResultBase>
    {

    }
    public class RequestHandlerLogDecoratorTest
    {
        private readonly Mock<IRequestHandler<RequestTestClass, ResultBase>> _decorated = new Mock<IRequestHandler<RequestTestClass, ResultBase>>();
        private readonly Mock<ILogger<RequestHandlerLogDecorator<RequestTestClass, ResultBase>>> _logger = new Mock<ILogger<RequestHandlerLogDecorator<RequestTestClass, ResultBase>>>();
        private RequestHandlerLogDecorator<RequestTestClass, ResultBase> _handlerDecorator;
        public RequestHandlerLogDecoratorTest()
        {
            _handlerDecorator = new RequestHandlerLogDecorator<RequestTestClass, ResultBase>(_decorated.Object, _logger.Object);
        }


        [Fact]
        public async Task Handle_HappyFlow_LogTimeRequired()
        {
            // Arrange
            RequestTestClass request = new RequestTestClass();
            ResultBase expectedResult = Results.Ok();
            _decorated.Setup(x => x.Handle(request, CancellationToken.None)).Returns(Task.FromResult(expectedResult));
            _logger.Setup(x => x.LogInformation(It.IsAny<string>()));

            // Act
            ResultBase result = await _handlerDecorator.Handle(request, CancellationToken.None);

            // Assert
            result.Should().Be(expectedResult);
            _logger.Verify(x => x.LogInformation(It.IsAny<string>()), Times.Once);
        }

        //[Fact]
        //public async Task Handle_Error_ThrowException()
        //{
        //    // Arrange
        //    RequestTestClass request = new RequestTestClass();
        //    ResultBase expectedResult = Results.Fail(new Error());
        //    _decorated.Setup(x => x.Handle(request, CancellationToken.None)).Returns(Task.FromResult(expectedResult));
        //    _logger.Setup(x => x.LogError(It.IsAny<string>()));

        //    // Act
        //    ResultBase result = await _handlerDecorator.Handle(request, CancellationToken.None);

        //    // Assert
        //    result.Should().Be(expectedResult);
        //    _logger.Verify(x => x.LogError(It.IsAny<string>()), Times.Exactly(2));
        //}

    }
}
