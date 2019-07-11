using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;
using TaskManagerAPI.BL.AuthProcess;
using TaskManagerAPI.Filters;
using TaskManagerAPI.Models.APIRequests;
using TaskManagerAPI.Models.FE;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }
        // POST api/<controller>
        [HttpPost("login")]
        public ActionResult Login(LoginRequest request)
        {
            _logger.LogInformation($"{request.Email}");
            PortalAccount accountResponse = this._authService.ValidateUser(request.Email, request.Password);
            if (accountResponse != null)
            {
                return Ok(accountResponse);
            }
            else
            {
                return BadRequest("Bad user or password");
            }
        }

        [HttpPost("logoff")]
        [AuthenticationFilter]
        public ActionResult LogOff()
        {
            int userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            this._authService.LogOff(userId);
            return Ok();
        }
    }
}
