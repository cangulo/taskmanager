using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using TaskManagerAPI.BL.AuthProcess;
using TaskManagerAPI.BL.UserVerifications;
using TaskManagerAPI.Resources.Constants;

namespace TaskManagerAPI.Filters
{
    public class AuthenticationFilterAttribute : TypeFilterAttribute
    {
        public AuthenticationFilterAttribute() : base(typeof(AuthenticationFilter)) { }
    }
    public class AuthenticationFilter : AuthorizeFilter
    {
        private readonly ITokenVerificator _tokenVerificator;
        private readonly IUserVerification _userVerification;
        public AuthenticationFilter(IAuthorizationPolicyProvider provider, ITokenVerificator tokenVerificator, IUserVerification userVerification) : base(provider, new[] { new AuthorizeData(ConfigurationConstants.JwtDefaultPolicy) })
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
                if (!_tokenVerificator.TokenIsValid(userId, token))
                {
                    context.Result = new UnauthorizedResult();
                }
                if (!_userVerification.UserIsActive(userId))
                {
                    context.Result = new UnauthorizedResult();
                }
            }
            return Task.CompletedTask;
        }
    }

    public class AuthorizeData : IAuthorizeData
    {
        public AuthorizeData() { }
        public AuthorizeData(string policy)
        {
            this.Policy = policy;
        }
        public string Policy { get; set; }
        public string Roles { get; set; }
        public string AuthenticationSchemes { get; set; }
    }
}

