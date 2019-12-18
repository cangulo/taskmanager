using Microsoft.AspNetCore.Mvc;

namespace TaskManagerAPI.Filters.Authentication
{
    public class AuthenticationFilterAttribute : TypeFilterAttribute
    {
        public AuthenticationFilterAttribute() : base(typeof(AuthenticationFilter))
        {
        }
    }
}