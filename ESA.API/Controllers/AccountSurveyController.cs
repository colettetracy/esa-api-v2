using ESA.Core.Interfaces;
using ESA.Core.Models.Account;
using GV.DomainModel.SharedKernel.Interop;
using Microsoft.AspNetCore.Mvc;

namespace ESA.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AccountSurveyController : ApiControllerBase
    {
        private readonly IAccountSurveyService accountSurveyService;

        public AccountSurveyController(IAccountSurveyService accountSurveyService)
        {
            this.accountSurveyService = accountSurveyService;
        }

        [HttpGet("{accountId}")]
        [ProducesResponseType(typeof(AccountSurveyInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<AccountSurveyInfo>>> GetSurveyAsync([FromRoute] int accountId)
        {
            return await accountSurveyService.FindByAccountAsync(accountId);
        }

        [HttpGet()]
        [ProducesResponseType(typeof(AccountSurveyInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<IEnumerable<AccountSurveyInfo>>>> GetAllSurveyAsync()
        {
            return await accountSurveyService.GetAllAsync();
        }

        [HttpPost]
        [ProducesResponseType(typeof(AccountSurveyInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<AccountSurveyInfo>>> AddAccountSurveyAsync([FromBody] AccountSurveyBaseInfo surveyInfo)
        {
            return await accountSurveyService.AddSurverAsync(surveyInfo);
        }

    }
}
