using ESA.Core.Interfaces;
using ESA.Core.Models.Course;
using ESA.Core.Models.Student;
using ESA.Core.Specs.Filters;
using GV.DomainModel.SharedKernel.Interop;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ESA.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class StudentGroupController : ControllerBase
    {
        private readonly IStudentGroupService studentService;

        public StudentGroupController(IStudentGroupService studentService)
        {
            this.studentService = studentService;
        }

        [HttpGet("filter")]
        [ProducesResponseType(typeof(IEnumerable<StudentGroupInfo>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<IEnumerable<StudentGroupInfo>>>> GetFilterStudentAsync([FromQuery] StudentGroupFilter filter)
        {
            return await studentService.FilterAsync(filter);
        }

        [HttpPost]
        [ProducesResponseType(typeof(StudentGroupInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<StudentGroupInfo>>> AddStudentAsync([FromBody] StudentGroupCreate studentInfo)
        {
            return await studentService.AddStudentAsync(studentInfo);
        }

        [HttpPut("confirmPayment")]
        [ProducesResponseType(typeof(StudentGroupInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<List<StudentGroupInfo>>>> UpdatePaymentAsync([FromBody] List<PaymentConfirmBaseInfo> studentInfo)
        {
            return await studentService.UpdatePaymentAsync(studentInfo);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ScheduleDeleteInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<ScheduleDeleteInfo>>> DeleteAsync([FromRoute] int Id)
        {
            return await studentService.DeleteStudentAsync(Id);
        }
    }
}
