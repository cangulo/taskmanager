using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Text.RegularExpressions;

namespace TaskManagerAPI.Test.Common.LoggerExtensions
{
    /// <summary>
    /// Thanks to: https://codeburst.io/unit-testing-with-net-core-ilogger-t-e8c16c503a80
    /// </summary>
    public static class LoggerCallsVerifier
    {
        public static void VerifyLog<T>(this Mock<ILogger<T>> loggerMock, LogLevel level, string message, Times times)
        {
            loggerMock
                .Verify(l =>
                    l.Log(
                        level,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((object v, Type _) => v.ToString() == message),
                        It.Is<Exception>(exp => exp == null),
                        (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                        times);
        }
        public static void VerifyLog<T>(this Mock<ILogger<T>> loggerMock, LogLevel level, Regex regex, Times times)
        {
            loggerMock
                .Verify(l =>
                    l.Log(
                        level,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((object v, Type _) => regex.IsMatch(v.ToString())),
                        It.IsAny<Exception>(),
                        (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                        times);
        }
    }
}
