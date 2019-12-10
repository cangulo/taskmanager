using FluentAssertions;
using TaskManagerAPI.Models.Errors;
using Xunit;

namespace TaskManagerAPI.Models.Test.Errors
{
    public class CustomErrorTest
    {
        private string _errorCodeTest = "errorCodeTest";
        private string _errorMessageTest = "errorMessageTest";
        private int _errorHttpCode404Test = 404;
        private int _errorHttpCode500Test = 500;

        [Fact]
        public void CreateCustomError_ValidMetadaProvidingHttpCode_ExpectedAllMetadata()
        {
            // Arrange
            // Act
            CustomError error = new CustomError(_errorCodeTest, _errorMessageTest, _errorHttpCode404Test);
            // Assert

            // Expected Error Code in the metadata
            error.Metadata.TryGetValue(ErrorKeyPropsConstants.ERROR_CODE, out object errorCode).Should().BeTrue();
            ((string)errorCode).Should().Be(_errorCodeTest);

            // Expected http error Code in the metadata
            error.Metadata.TryGetValue(ErrorKeyPropsConstants.ERROR_HTTP_CODE, out object httpErrorcode).Should().BeTrue();
            ((int)httpErrorcode).Should().Be(_errorHttpCode404Test);

            // Expected error message
            error.Message.Should().Be(_errorMessageTest);
        }
    }
}
