using FluentAssertions;
using TaskManagerAPI.Models.Errors;
using Xunit;

namespace TaskManagerAPI.Models.Test.Errors
{
    public class ErrorToHttpStatusCodeHelperTest
    {
        private ErrorToHttpStatusCodeHelper _helper = new ErrorToHttpStatusCodeHelper();

        [Fact]
        public void DefaultError_Return_500()
        {
            // Arrange
            string errorCode = "XXXXX";
            // Act
            int httpCode = _helper.ToHttpStatusCode(errorCode);
            // Assert
            httpCode.Should().Be(500);
        }

        [Fact]
        public void AllApiLayerErrors_Return_500()
        {
            // Arrange
            string errorCode = "0XXXX";
            // Act
            int httpCode = _helper.ToHttpStatusCode(errorCode);
            // Assert
            httpCode.Should().Be(500);
        }
    }
}
