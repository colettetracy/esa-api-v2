using ESA.Core.Models.Student;
using ESA.Core.Services;
using ESA.Core.Specs.Filters;
using GV.DomainModel.SharedKernel.Interop;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ESA.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {

        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }
    }
}
