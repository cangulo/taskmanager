﻿using FluentAssertions;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Test.Common.JsonFileDataAttribute;
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

        [Theory]
        [JsonFileDataAttribute(@".\Errors\Resources\errorHttpCodes.json", "BL_Layer_Errors")]
        public void TestBL_Layer_ExpectedResult(string errorCode, int expectedHttpCode)
        {
            // Arrange
            // Act
            int httpCode = _helper.ToHttpStatusCode(errorCode);
            // Assert
            httpCode.Should().Be(expectedHttpCode);
        }
    }
}