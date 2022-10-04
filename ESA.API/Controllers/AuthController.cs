using ESA.Core.Interfaces;
using ESA.Core.Models.Auth;
using GV.DomainModel.SharedKernel.Interop;
using Microsoft.AspNetCore.Mvc;

namespace ESA.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : ApiControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserAccountInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<UserAccountInfo>>> UserLogin([FromBody] UserCredential model)
        {
            return await authService.UserAuthenticationAsync(model);
        }
    }
}
