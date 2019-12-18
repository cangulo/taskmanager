using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.IO;
using System.Threading.Tasks;
using TaskManagerAPI.Exceptions;
using TaskManagerAPI.Exceptions.Handlers;
using TaskManagerAPI.Pipeline;
using Xunit;

namespace TaskManagerAPI.Test.Middleware
{
    public class ExceptionHandlerMiddlewareTest
    {
        [Fact]
        public async Task ExceptionHandlerMiddleware_CatchException()
        {
            // Arrange
            var exHandler = new Mock<IExceptionHandler>();
            int expectedStatusCode = 500;
            string expectedResponseContent = "test";
            exHandler.Setup(x => x.CreateResponseContent()).Returns(expectedResponseContent);
            exHandler.Setup(x => x.GetHttpStatusCode()).Returns(expectedStatusCode);

            var exHandlerFactory = new Mock<IExceptionHandlerFactory>();

            Exception ex = new Exception();
            exHandlerFactory.Setup(x => x.GetExceptionHandler(ex)).Returns(exHandler.Object);

            var middleware = new ExceptionHandlerMiddleware(exHandlerFactory.Object, (innerHttpContext) =>
            {
                throw ex;
            });

            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            // Act
            await middleware.Invoke(context);

            // Assert
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(context.Response.Body);
            var responseContent = reader.ReadToEnd();
            responseContent.Should().BeEquivalentTo(expectedResponseContent);
            context.Response.StatusCode.Should().Be(expectedStatusCode);
        }
    }
}