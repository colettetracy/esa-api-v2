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

        [HttpGet("availability")]
        [ProducesResponseType(typeof(IEnumerable<ScheduleInfo>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<IEnumerable<AvailabilityInfo>>>> GetAvailabilityScheduleAsync([FromQuery] ScheduleFilter filter)
        {
            var filtered = await scheduleService.FilterAsync(filter);


            return UtilsAvailability.getAvailability(filtered.Value);
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
    public static class UtilsAvailability
    {
        public static Result<IEnumerable<AvailabilityInfo>> getAvailability(IEnumerable<ScheduleInfo> schedule)
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
                        availabilityInfo.Times.AddRange(getTimesOfDay(sch.Schedule, sch.Minutes));
                    }
                    else
                    {
                        if(!existsDate(availability.Select(x=> x.Date).ToList(), sch.Schedule))
                        {
                            availabilityInfo.Date = sch.Schedule;
                        }
                        else
                        {
                            availabilityInfo = availability.Where(x => x.Date.Equals(sch.Schedule)).FirstOrDefault();
                        }
                        availabilityInfo.Times.AddRange(getTimesOfDay(sch.Schedule, sch.Minutes));
                    }
                    availability.Add(availabilityInfo);
                }

                return result.Success(availability);
            }
            catch (Exception ex)
            {
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }

        public static List<string> getTimesOfDay(DateTime date, int duration)
        {
            var dateTmp = date;
            List<string> times = new List<string>();
            while (dateTmp.CompareTo(date.AddMinutes(duration)) < 0)
            {
                times.Add(dateTmp.ToShortTimeString()+"");
                dateTmp = dateTmp.AddMinutes(30);
            }
            return times;
        }

        public static bool existsDate(List<DateTime?> dates, DateTime date)
        {
            return dates.Where(d => d.Equals(date)).Any();
        }
    }
}
