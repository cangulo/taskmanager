using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TaskManagerAPI.Exceptions.Helpers;
using Xunit;

namespace TaskManagerAPI.Test.Exceptions.Helpers
{
    public class ErrorResponseCreatorTest
    {
        [Fact]
        public void CreateResponse_EmptyListErrors_ReturnDefaultError()
        {
            // Arrange
            ErrorResponseCreator errorResponseCreator = new ErrorResponseCreator();
            List<Error> emptyErrorList = new List<Error>();
            // Act
            //IActionResult actionResult = errorResponseCreator.CreateResponse(emptyErrorList);
            // Assert
        }
    }
}
