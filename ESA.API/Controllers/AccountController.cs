using ESA.Core.Interfaces;
using ESA.Core.Models.Account;
using ESA.Core.Models.Course;
using ESA.Core.Services;
using GV.DomainModel.SharedKernel.Interop;
using Microsoft.AspNetCore.Mvc;

namespace ESA.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AccountController : ApiControllerBase
    {
        private readonly IAccountService accountService;
        private readonly IAccountProfileService profileService;

        public AccountController(IAccountService accountService, IAccountProfileService profileService)
        {
            this.accountService = accountService;
            this.profileService = profileService;
        }

        [HttpGet("{accountId}")]
        [ProducesResponseType(typeof(AccountInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<AccountInfo>>> GetAccountByIdAsync([FromRoute] int accountId)
        {
            return await accountService.FindByIdAsync(accountId);
        }

        [HttpGet("by-role/{roleId}")]
        [ProducesResponseType(typeof(IEnumerable<AccountInfo>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<IEnumerable<AccountInfo>>>> GetAccountsByRoleAsync([FromRoute] short roleId)
        {
            return await accountService.FindByRoleAsync(roleId);
        }

        [HttpGet("by-email/{email}")]
        [ProducesResponseType(typeof(AccountInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<AccountInfo>>> GetAccountByEmailAsync([FromRoute] string email)
        {
            return await accountService.FindByEmailAsync(email);
        }

        [HttpPost]
        [ProducesResponseType(typeof(AccountInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<AccountInfo>>> AddAccountAsync([FromBody] AccountBaseInfo accountBaseInfo)
        {
            return await accountService.AddAccountAsync(accountBaseInfo);
        }

        [HttpPut("{accountId}")]
        [ProducesResponseType(typeof(AccountInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<AccountInfo>>> UpdateAccountAsync([FromRoute] int accountId, [FromBody] AccountBaseInfo accountBaseInfo)
        {
            return await accountService.UpdateAccountAsync(accountId, accountBaseInfo);
        }

        [HttpDelete("{accountId}")]
        [ProducesResponseType(typeof(ScheduleDeleteInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<ScheduleDeleteInfo>>> DeleteCourseAsync([FromRoute] int accountId)
        {
            return await accountService.DeleteAsync(accountId);
        }

        #region Profile
        [HttpGet("profile/{accountId}")]
        [ProducesResponseType(typeof(AccountProfileInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<AccountProfileInfo>>> GetProfileByIdAsync([FromRoute] int accountId)
        {
            return await profileService.FindByAccountAsync(accountId);
        }

        [HttpPost("profile")]
        [ProducesResponseType(typeof(AccountProfileInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<AccountProfileInfo>>> AddProfileAsync([FromBody] AccountProfileBase profileInfo)
        {
            return await profileService.AddProfileAsync(profileInfo);
        }

        [HttpPut("profile/{profileId}")]
        [ProducesResponseType(typeof(AccountProfileInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<AccountProfileInfo>>> AddProfileAsync([FromRoute] int profileId, [FromBody] AccountProfileBase profileInfo)
        {
            return await profileService.UpdateProfileAsync(profileId, profileInfo);
        }
        #endregion
    }
}
