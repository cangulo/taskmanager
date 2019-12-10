using FluentResults;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Resources.Errors;

namespace TaskManagerAPI.BL.CurrentUserService
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Result<int> GetIdCurrentUser()
        {
            string claimValue = _httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (!string.IsNullOrEmpty(claimValue) && int.TryParse(claimValue, out int userId))
            {
                return Results.Ok(userId);
            }
            return Results.Fail<int>(
                new CustomError(ErrorsCodesContants.CURRENT_USER_ID_NOT_FOUND, ErrorsMessagesConstants.CURRENT_USER_ID_NOT_FOUND, 404));
        }
    }
}
