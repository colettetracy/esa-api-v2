using ESA.Core.Interfaces;
using ESA.Core.Models.Course;
using ESA.Core.Specs.Filters;
using GV.DomainModel.SharedKernel.Interop;
using Microsoft.AspNetCore.Mvc;

namespace ESA.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CourseController : ApiControllerBase
    {
        private readonly ICourseService courseService;

        public CourseController(ICourseService courseService)
        {
            this.courseService = courseService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CourseInfo>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<IEnumerable<CourseInfo>>>> GetAllCourseAsync()
        {
            return await courseService.GetAllAsync();
        }

        [HttpGet("filter")]
        [ProducesResponseType(typeof(IEnumerable<CourseInfo>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<IEnumerable<CourseInfo>>>> GetFilterCourseAsync([FromQuery] CourseFilter filter)
        {
            return await courseService.FilterAsync(filter);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CourseInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<CourseInfo>>> AddCourseAsync([FromBody] CourseBaseInfo courseInfo)
        {
            return await courseService.AddAsync(courseInfo);
        }

        [HttpPut("{courseId}")]
        [ProducesResponseType(typeof(CourseInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<CourseInfo>>> UpdateAccountAsync([FromRoute] int courseId, [FromBody] CourseBaseInfo courseInfo)
        {
            return await courseService.UpdateAsync(courseId, courseInfo);
        }

        [HttpDelete("{courseId}")]
        [ProducesResponseType(typeof(ScheduleDeleteInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<ScheduleDeleteInfo>>> DeleteCourseAsync([FromRoute] int courseId)
        {
            return await courseService.DeleteAsync(courseId);
        }
    }
}
