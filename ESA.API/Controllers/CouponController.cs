using ESA.Core.Interfaces;
using ESA.Core.Models.Course;
using ESA.Core.Models.Payment;
using ESA.Core.Services;
using GV.DomainModel.SharedKernel.Interop;
using Microsoft.AspNetCore.Mvc;

namespace ESA.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CouponController : ApiControllerBase
    {
        private readonly ICouponService couponService;

        public CouponController(ICouponService couponService)
        {
            this.couponService = couponService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CouponInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<IEnumerable<CouponInfo>>>> GetAllCouponsAsync()
        {
            return await couponService.GetAllAsync();
        }

        [HttpGet("{couponId}")]
        [ProducesResponseType(typeof(CouponInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<CouponInfo>>> GetCouponByIdAsync([FromRoute] int couponId)
        {
            return await couponService.FindByIdAsync(couponId);
        }


        [HttpGet("by-code/{code}")]
        [ProducesResponseType(typeof(CouponInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<CouponInfo>>> GetCouponByCodeAsync([FromRoute] string code)
        {
            return await couponService.FindByCodeAsync(code);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CouponInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<CouponInfo>>> AddCouponAsync([FromBody] CouponBaseInfo couponBaseInfo)
        {
            return await couponService.AddCouponAsync(couponBaseInfo);
        }

        [HttpDelete("{couponId}")]
        [ProducesResponseType(typeof(ScheduleDeleteInfo), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<ScheduleDeleteInfo>>> DeleteCourseAsync([FromRoute] int couponId)
        {
            return await couponService.DeleteCouponAsync(couponId);
        }

    }
}
