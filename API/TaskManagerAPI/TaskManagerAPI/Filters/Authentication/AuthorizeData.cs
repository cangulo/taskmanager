using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace TaskManagerAPI.Filters.Authentication
{
    /// <summary>
    /// Necessary for the constructor of <see cref="AuthorizeFilter"/> used in <see cref="AuthenticationFilter"/>
    /// </summary>
    public class AuthorizeData : IAuthorizeData
    {
        public AuthorizeData()
        {
        }

        public AuthorizeData(string policy)
        {
            this.Policy = policy;
        }

        public string Policy { get; set; }
        public string Roles { get; set; }
        public string AuthenticationSchemes { get; set; }
    }
}