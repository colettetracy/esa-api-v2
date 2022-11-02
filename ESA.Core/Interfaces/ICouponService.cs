using ESA.Core.Models.Course;
using ESA.Core.Models.Payment;
using GV.DomainModel.SharedKernel.Interop;

namespace ESA.Core.Interfaces
{
    public interface ICouponService
    {
        Task<Result<IEnumerable<CouponInfo>>> GetAllAsync();
        Task<Result<CouponInfo>> FindByIdAsync(int couponId);

        Task<Result<CouponInfo>> FindByCodeAsync(string code);

        Task<Result<CouponInfo>> AddCouponAsync(CouponBaseInfo couponBaseInfo);

        Task<Result<ScheduleDeleteInfo>> DeleteCouponAsync(int couponId);
    }
}
