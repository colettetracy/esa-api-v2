using ESA.Core.Interfaces;
using ESA.Core.Models.Student;
using ESA.Core.Specs.Filters;
using GV.DomainModel.SharedKernel.Interop;
using Microsoft.AspNetCore.Mvc;

namespace ESA.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class StudentController : ApiControllerBase
    {
        private readonly IStudentService studentService;

        public StudentController(IStudentService studentService)
        {
            this.studentService = studentService;
        }

        [HttpGet("filter")]
        [ProducesResponseType(typeof(IEnumerable<StudentInfo>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<IEnumerable<StudentInfo>>>> GetFilterStudentAsync([FromQuery] StudentFilter filter)
        {
            return await studentService.FilterAsync(filter);
        }

        [HttpPost]
        [ProducesResponseType(typeof(StudentInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<StudentInfo>>> AddStudentAsync([FromBody] StudentBaseInfo studentInfo)
        {
            return await studentService.AddStudentAsync(studentInfo);
        }
    }
}
