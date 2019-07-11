using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using TaskManagerAPI.EF.Context;
using TaskManagerAPI.Resources.AppSettings;

namespace TaskManagerAPI.BL.AuthProcess
{
    public class TokenVerificator : ITokenVerificator
    {
        private readonly ITaskManagerDbContext _taskManagerDbContext;
        private readonly AppSettings _appSettings;

        public TokenVerificator(ITaskManagerDbContext taskManagerDbContext, IOptions<AppSettings> options)
        {
            _taskManagerDbContext = taskManagerDbContext;
            _appSettings = options.Value;
        }

        public bool TokenIsValid(int userId, string token)
        {
            return this._taskManagerDbContext.Accounts.First(ac => ac.Id == userId).Token == token;
        }
    }
}
