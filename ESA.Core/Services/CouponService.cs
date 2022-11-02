using AutoMapper;
using ESA.Core.Interfaces;
using ESA.Core.Models.Payment;
using ESA.Core.Models.Course;
using ESA.Core.Specs;
using ESA.Core.Validators;
using GV.DomainModel.SharedKernel.Extensions;
using GV.DomainModel.SharedKernel.Interfaces;
using GV.DomainModel.SharedKernel.Interop;

namespace ESA.Core.Services
{
    public class CouponService : ICouponService
    {
        private readonly IMapper mapper;
        private readonly IAppLogger<CouponService> logger;
        private readonly IRepository<Coupons> couponWriteRepository;
        private readonly IReadRepository<Coupons> couponReadRepository;

        public CouponService(IMapper mapper, IAppLogger<CouponService> logger, IRepository<Coupons> couponWriteRepository, IReadRepository<Coupons> couponReadRepository)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.couponWriteRepository = couponWriteRepository;
            this.couponReadRepository = couponReadRepository;
        }

        public async Task<Result<CouponInfo>> AddCouponAsync(CouponBaseInfo couponBaseInfo)
        {
            var result = new Result<CouponInfo>();
            try
            {
                if (couponBaseInfo == null)
                    return result.Invalid(new List<ValidationError>
                    {
                        new ValidationError()
                        {
                            Identifier = $"Coupon::{AddCouponAsync}",
                            ErrorMessage = "Model invalid",
                            Severity = ValidationSeverity.Warning
                        }
                    });

                var validatorInfo = new CouponValidators();
                var validationInfo = validatorInfo.Validate(couponBaseInfo);
                if (!validationInfo.IsValid)
                    return result.Invalid(validationInfo.AsErrors());

                var coupon = mapper.Map<Coupons>(couponBaseInfo);
                if (coupon == null)
                    return result.Conflict("Mapping error");

                coupon.IsActive = true;
                
                coupon = await couponWriteRepository.AddAsync(coupon, Utils.Commons.GetCancellationToken(15).Token);
                if (coupon == null)
                    return result.Conflict("Save error");

                return result.Success(mapper.Map<CouponInfo>(coupon));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, couponBaseInfo);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }

        public async Task<Result<ScheduleDeleteInfo>> DeleteCouponAsync(int couponId)
        {
            var result = new Result<ScheduleDeleteInfo>();
            try
            {
                var exists = await couponReadRepository.FirstOrDefaultAsync(new CouponSpec(couponId));
                if (exists == null)
                    return result.NotFound("");
                exists.IsActive = false;
                await couponWriteRepository.UpdateAsync(exists);
                return result.Success(new ScheduleDeleteInfo
                {
                    Deleted = true
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, couponId);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }

        public async Task<Result<CouponInfo>> FindByCodeAsync(string code)
        {
            var result = new Result<CouponInfo>();
            try
            {
                if (string.IsNullOrWhiteSpace(code))
                    return result.Invalid(new List<ValidationError>
                    {
                        new ValidationError()
                        {
                            Identifier = $"Coupon::{nameof(FindByCodeAsync)}",
                            ErrorMessage = "Code invalid",
                            Severity = ValidationSeverity.Warning
                        }
                    });

                var coupon = await couponReadRepository.FirstOrDefaultAsync(new CouponSpec(code), Utils.Commons.GetCancellationToken(15).Token);
                if (coupon == null)
                    return result.NotFound($"Coupon not exist: {code}");

                return result.Success(mapper.Map<CouponInfo>(coupon));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, code);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }

        public async Task<Result<CouponInfo>> FindByIdAsync(int couponId)
        {
            var result = new Result<CouponInfo>();
            try
            {
                if (couponId <= 0)
                    return result.Invalid(new List<ValidationError>
                    {
                        new ValidationError()
                        {
                            Identifier = $"Coupon::{nameof(FindByIdAsync)}",
                            ErrorMessage = "CouponId invalid",
                            Severity = ValidationSeverity.Warning
                        }
                    });

                var coupon = await couponReadRepository.FirstOrDefaultAsync(new CouponSpec(couponId), Utils.Commons.GetCancellationToken(15).Token);
                if (coupon == null)
                    return result.NotFound($"Coupon not exist: {couponId}");

                return result.Success(mapper.Map<CouponInfo>(coupon));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, couponId);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }

        public async Task<Result<IEnumerable<CouponInfo>>> GetAllAsync()
        {
            var result = new Result<IEnumerable<CouponInfo>>();
            try
            {
                var list = await couponReadRepository.ListAsync();
                if (list == null)
                    return result.NotFound("");

                //list.Where(x => x.IsActive == true);
                return result.Success(list.Select(x => mapper.Map<CouponInfo>(x)).Where(x => x.IsActive == true));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }
    }
}
