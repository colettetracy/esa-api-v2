using ESA.Core.Interfaces;
using ESA.Core.Models.Course;
using ESA.Core.Models.Payment;
using ESA.Core.Specs.Filters;
using GV.DomainModel.SharedKernel.Interop;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ESA.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            this.dashboardService = dashboardService;
        }

        [HttpGet()]
        [ProducesResponseType(typeof(DashboardInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<DashboardInfo>>> GetFilterReviewAsync()
        {
            return await dashboardService.GetInfo();
        }
    }
}
