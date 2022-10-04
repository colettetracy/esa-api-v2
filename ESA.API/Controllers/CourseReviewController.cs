using ESA.Core.Interfaces;
using ESA.Core.Models.Course;
using ESA.Core.Specs.Filters;
using GV.DomainModel.SharedKernel.Interop;
using Microsoft.AspNetCore.Mvc;

namespace ESA.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CourseReviewController : ApiControllerBase
    {
        private readonly IReviewService reviewService;

        public CourseReviewController(IReviewService reviewService)
        {
            this.reviewService = reviewService;
        }

        [HttpGet("filter")]
        [ProducesResponseType(typeof(IEnumerable<ReviewInfo>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<IEnumerable<ReviewInfo>>>> GetFilterReviewAsync([FromQuery] ReviewFilter filter)
        {
            return await reviewService.FilterAsync(filter);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ReviewInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<ReviewInfo>>> AddReviewAsync([FromBody] ReviewBaseInfo reviewInfo)
        {
            return await reviewService.AddReviewAsync(reviewInfo);
        }
    }
}
