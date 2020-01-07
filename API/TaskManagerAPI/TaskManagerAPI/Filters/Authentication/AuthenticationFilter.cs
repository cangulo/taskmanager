using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManagerAPI.BL.AuthProcess;
using TaskManagerAPI.BL.UserStatusVerification;
using TaskManagerAPI.Resources.Constants;

namespace TaskManagerAPI.Filters.Authentication
{
    public class AuthenticationFilter : AuthorizeFilter
    {
        private readonly ITokenVerificator _tokenVerificator;
        private readonly IUserStatusVerification _userVerification;

        public AuthenticationFilter(IAuthorizationPolicyProvider provider, ITokenVerificator tokenVerificator, IUserStatusVerification userVerification) : base(provider, new[] { new AuthorizeData(ConfigurationConstants.JwtDefaultPolicy) })
        {
            this._tokenVerificator = tokenVerificator;
            this._userVerification = userVerification;
        }

        public override Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            base.OnAuthorizationAsync(context);

            bool thereAreClaims = context.HttpContext.User.Claims.Count() > 0;
            if (context.HttpContext.Request.Headers.ContainsKey("Authorization") && thereAreClaims)
            {
                string token = ((string)context.HttpContext.Request.Headers["Authorization"]).Replace("Bearer ", "");

                int userId = int.Parse(context.HttpContext.User.Claims.First(cl => cl.Type == ClaimTypes.NameIdentifier).Value);

                // TODO: Return 401 and include the custom error object we use in the project with the error message and error code

                if (!_tokenVerificator.TokenIsValid(userId, token))
                {
                    context.Result = new UnauthorizedResult();
                }
                Result userIsActiveQuery = _userVerification.UserIsActive(userId);
                if (!userIsActiveQuery.IsSuccess)
                {
                    context.Result = new UnauthorizedResult();
                }
                // TODO: Return error content of the failed result
                //else
                //{
                //    context.Result = new UnauthorizedResult();
                //}
            }
            return Task.CompletedTask;
        }
    }
}