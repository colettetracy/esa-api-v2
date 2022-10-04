using ESA.Core.Interfaces;
using ESA.Core.Models.Course;
using ESA.Core.Services;
using GV.DomainModel.SharedKernel.Interop;
using Microsoft.AspNetCore.Mvc;

namespace ESA.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CourseCalendarController : ApiControllerBase
    {
        private readonly ICalendarService calendarService;

        public CourseCalendarController(ICalendarService calendarService)
        {
            this.calendarService = calendarService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CalendarInfo>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<IEnumerable<CalendarInfo>>>> GetAllCalendarAsync()
        {
            return await calendarService.GetAllAsync();
        }

        [HttpGet("by-course/{courseId}")]
        [ProducesResponseType(typeof(IEnumerable<CalendarInfo>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<IEnumerable<CalendarInfo>>>> GetAllCalendarByCourseAsync([FromRoute] int courseId)
        {
            return await calendarService.FindByCourseAsync(courseId);
        }

        [HttpGet("by-teacher/{teacherId}")]
        [ProducesResponseType(typeof(IEnumerable<CalendarInfo>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<IEnumerable<CalendarInfo>>>> GetAllCalendarByTeacherAsync([FromRoute] int teacherId)
        {
            return await calendarService.FindByTeacherAsync(teacherId);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CalendarInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<CalendarInfo>>> AddCalendarAsync([FromBody] CalendarBaseInfo calendarInfo)
        {
            return await calendarService.AddCalendarAsync(calendarInfo);
        }
    }
}
