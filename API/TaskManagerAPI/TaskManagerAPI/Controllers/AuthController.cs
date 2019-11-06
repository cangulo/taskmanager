using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskManagerAPI.BL.AuthProcess;
using TaskManagerAPI.Filters.Authentication;
using TaskManagerAPI.Helpers;
using TaskManagerAPI.Models.FE;
using TaskManagerAPI.Models.FE.APIRequests;

namespace TaskManagerAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly IErrorResponseCreator _errorResponseCreator;

        public AuthController(IAuthService authService, ILogger<AuthController> logger, IErrorResponseCreator errorResponseCreator)
        {
            _authService = authService;
            _logger = logger;
            _errorResponseCreator = errorResponseCreator;
        }

        /// <summary>
        /// Log in the user 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <response code="200">User Logged correctly</response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Login([FromBody]LoginRequest request)
        {
            _logger.LogInformation($"{request.Email}");
            Result<PortalAccount> result = this._authService.ValidateUser(request.Email, request.Password);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            else
            {
                return _errorResponseCreator.CreateResponse(result.Errors);
            }
        }

        /// <summary>
        /// Sign up a new user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <response code="200">User Sign up correctly</response>
        [HttpPost("signup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult SignUp([FromBody]SignUpRequest request)
        {
            _logger.LogInformation($"{request.Email}");
            Result result = this._authService.SignUpUser(request.FullName, request.Email, request.Password);
            if (result.IsSuccess)
            {
                return Ok();
            }
            else
            {
                return _errorResponseCreator.CreateResponse(result.Errors);
            }
        }

        /// <summary>
        /// Log off the current user
        /// </summary>
        /// <returns></returns>
        /// <response code="200">User Session ended correctly</response>
        [HttpPost("logoff")]
        [AuthenticationFilter]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult LogOff()
        {
            Result opLoggOffresult = this._authService.LogOff();
            if (opLoggOffresult.IsSuccess)
            {
                return Ok();
            }
            else
            {
                return _errorResponseCreator.CreateResponse(opLoggOffresult.Errors);

            }

        }
    }
}
