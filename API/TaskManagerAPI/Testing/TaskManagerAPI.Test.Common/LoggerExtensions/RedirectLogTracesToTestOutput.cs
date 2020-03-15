using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit.Abstractions;

namespace TaskManagerAPI.Test.Common.LoggerExtensions
{
    public static class RedirectLogTracesToTestOutput
    {
        public static void RedirectLogOutputToTestOutput<T>(this Mock<ILogger<T>> loggerMock, ITestOutputHelper testOutputHelper)
        {
            loggerMock
                .Setup(l =>
                    l.Log(
                        It.IsAny<LogLevel>(),
                        It.IsAny<EventId>(),
                        It.IsAny<It.IsAnyType>(),
                        It.IsAny<Exception>(),
                        (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()))
                .Callback<LogLevel, EventId, object, Exception, object>((logLevel, eventId, message, exception, formatter) =>
                {
                    testOutputHelper.WriteLine(message.ToString());
                });
        }
    }
}
