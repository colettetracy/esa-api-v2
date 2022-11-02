using AutoMapper;
using ESA.Core.Interfaces;
using ESA.Core.Models.Course;
using ESA.Core.Specs;
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
    public class UtilsAvailability
    {
        public Result<IEnumerable<AvailabilityInfo>> getAvailability(IEnumerable<ScheduleInfo> schedule)
        {
            var result = new Result<IEnumerable<AvailabilityInfo>>();
            try
            {
                List<AvailabilityInfo>? availability = new();
                foreach (ScheduleInfo sch in schedule)
                {
                    AvailabilityInfo availabilityInfo = new AvailabilityInfo();
                    if (availability.Count() == 0)
                    {
                        availabilityInfo.Date = sch.Schedule;
                        availabilityInfo.Times.Add(sch.Schedule.ToShortTimeString());
                    }
                    else
                    {

                    }
                }

                return result.Success(availability);
            }
            catch (Exception ex)
            {
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }
    }
}
