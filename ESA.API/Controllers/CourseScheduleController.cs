using ESA.Core.Interfaces;
using ESA.Core.Models.Course;
using ESA.Core.Specs.Filters;
using GV.DomainModel.SharedKernel.Interop;
using Microsoft.AspNetCore.Mvc;

namespace ESA.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CourseScheduleController : ApiControllerBase
    {
        private readonly IScheduleService scheduleService;

        public CourseScheduleController(IScheduleService scheduleService)
        {
            this.scheduleService = scheduleService;
        }

        [HttpGet("filter")]
        [ProducesResponseType(typeof(IEnumerable<ScheduleInfo>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<IEnumerable<ScheduleInfo>>>> GetFilterScheduleAsync([FromQuery] ScheduleFilter filter)
        {
            return await scheduleService.FilterAsync(filter);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ScheduleInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<ScheduleInfo>>> AddScheduleAsync([FromBody] ScheduleBaseInfo scheduleInfo)
        {
            return await scheduleService.AddScheduleAsync(scheduleInfo);
        }

        [HttpDelete("{scheduleId}")]
        [ProducesResponseType(typeof(ScheduleDeleteInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<ScheduleDeleteInfo>>> DeleteScheduleAsync([FromRoute] int scheduleId)
        {
            return await scheduleService.DeleteScheduleAsync(scheduleId);
        }
    }
}
