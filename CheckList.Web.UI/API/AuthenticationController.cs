using Infrastructure.Implementation;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace CheckList.Web.UI.API
{
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationService _authenticationService;

        public AuthenticationController(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }


        [HttpPost("registeruser")]
        public string RegisterUser([FromBody]UserRegistrationEditModel model)
        {
            return _authenticationService.RegisterUser(model);
        }


        [HttpPost("authenticateuser")]
        public UserSessionModel AuthenticateUser([FromBody]UserAuthenticationEditModel model)
        {
            return _authenticationService.AuthenticateUser(model);
        }
    }
}