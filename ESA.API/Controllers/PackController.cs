using ESA.Core.Interfaces;
using ESA.Core.Models.Course;
using ESA.Core.Models.Payment;
using GV.DomainModel.SharedKernel.Interop;
using Microsoft.AspNetCore.Mvc;

namespace ESA.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PacksController : ApiControllerBase
    {
        private readonly IPackService packService;

        public PacksController(IPackService packService)
        {
            this.packService = packService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PackInfo>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<IEnumerable<PackInfo>>>> GetAllPackAsync()
        {
            return await packService.GetAllAsync();
        }

        [HttpPost]
        [ProducesResponseType(typeof(PackInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<PackInfo>>> AddPackAsync([FromBody] PackBaseInfo packInfo)
        {
            return await packService.AddPackAsync(packInfo);
        }

        [HttpPut("{packId}")]
        [ProducesResponseType(typeof(PackInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<PackInfo>>> UpdateAccountAsync([FromRoute] int packId, [FromBody] PackBaseInfo packInfo)
        {
            return await packService.UpdatePackAsync(packInfo, packId);
        }

        [HttpDelete("{packId}")]
        [ProducesResponseType(typeof(ScheduleDeleteInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<ScheduleDeleteInfo>>> DeletePackAsync([FromRoute] int packId)
        {
            return await packService.DeletePackAsync(packId);
        }
    }
}
