using AutoMapper;
using ESA.Core.Interfaces;
using ESA.Core.Models.Course;
using ESA.Core.Models.Student;
using ESA.Core.Services;
using ESA.Core.Specs;
using ESA.Core.Specs.Filters;
using GV.DomainModel.SharedKernel.Interop;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ESA.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CourseScheduleController : ApiControllerBase
    {
        private readonly IScheduleService scheduleService;
        private readonly IStudentService studentService;

        public CourseScheduleController(IScheduleService scheduleService, IStudentService studentService)
        {
            this.scheduleService = scheduleService;
            this.studentService = studentService;
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
            StudentFilter filter2 = new();
            filter2.TeacherId = filter.TeacherId;
            filter2.PaymentConfirmed = true;

            var filtered = await scheduleService.FilterAsync(filter);
            var purchases = await studentService.FilterAsync(filter2);

            return UtilsAvailability.getAvailability(filtered.Value, purchases.Value);
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
        public static Result<IEnumerable<AvailabilityInfo>> getAvailability(IEnumerable<ScheduleInfo> schedule, IEnumerable<StudentInfo> student)
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
                        availabilityInfo.Date = sch.Schedule;

                        availabilityInfo?.Times.AddRange(getTimesOfDay(sch.Schedule, sch.Minutes));
                    }
                    var finalDates = removeTimes(student.ToList(), availabilityInfo);
                    finalDates.scheduleInfo = sch;
                    if (finalDates.Times.Count() > 0)
                        availability.Add(finalDates);
                }

                return result.Success(availability);
            }
            catch (Exception ex)
            {
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }

        public static AvailabilityInfo removeTimes(List<StudentInfo> purchased, AvailabilityInfo availability)
        {
            foreach (var item in purchased)
            {
                var times = getTimesOfDay(item.EnrolledDate, item.TimePurchased);
                var date = item.EnrolledDate.ToShortDateString();

                if (availability.Date.Value.ToShortDateString().Equals(date))
                {
                    foreach (var timesAv in availability.Times)
                    {
                        if (times.Any(x=> x.Equals(timesAv)))
                            availability.Times = availability.Times.Where(x=> !x.Equals(timesAv)).ToList();
                    }

                }
            }
            return availability;
        }

        public static List<DateTime> getTimesOfDay(DateTime date, int duration)
        {
            var dateTmp = date;
            List<DateTime> times = new List<DateTime>();
            while (dateTmp.CompareTo(date.AddMinutes(duration)) < 0)
            {
                times.Add(dateTmp);
                dateTmp = dateTmp.AddMinutes(30);
            }
            return times;
        }

        public static bool existsDate(List<DateTime?> dates, DateTime date)
        {
            var dtmp = dates.Select(d => d.Value.ToShortDateString()).Contains(date.ToShortDateString());

            return dtmp;
        }
    }
}
