using AutoMapper;
using ESA.Core.Interfaces;
using ESA.Core.Models.Payment;
using ESA.Core.Models.Course;
using ESA.Core.Specs;
using ESA.Core.Validators;
using GV.DomainModel.SharedKernel.Extensions;
using GV.DomainModel.SharedKernel.Interfaces;
using GV.DomainModel.SharedKernel.Interop;
using ESA.Core.Models.Student;

namespace ESA.Core.Services
{
    public class PackService : IPackService
    {
        private readonly IMapper mapper;
        private readonly IAppLogger<PackService> logger;
        private readonly IRepository<LessonsPack> packWriteRepository;
        private readonly IReadRepository<LessonsPack> packReadRepository;

        public PackService(IMapper mapper, IAppLogger<PackService> logger, IRepository<LessonsPack> packWriteRepository, IReadRepository<LessonsPack> packReadRepository)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.packWriteRepository = packWriteRepository;
            this.packReadRepository = packReadRepository;
        }

        public async Task<Result<PackInfo>> AddPackAsync(PackBaseInfo packBaseInfo)
        {
            var result = new Result<PackInfo>();
            try
            {
                if (packBaseInfo == null)
                    return result.Invalid(new List<ValidationError>
                    {
                        new ValidationError()
                        {
                            Identifier = $"Pack::{AddPackAsync}",
                            ErrorMessage = "Model invalid",
                            Severity = ValidationSeverity.Warning
                        }
                    });

                var pack = mapper.Map<LessonsPack>(packBaseInfo);
                if (pack == null)
                    return result.Conflict("Mapping error");

                pack.IsActive = true;
                
                pack = await packWriteRepository.AddAsync(pack, Utils.Commons.GetCancellationToken(15).Token);
                if (pack == null)
                    return result.Conflict("Save error");

                return result.Success(mapper.Map<PackInfo>(pack));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, packBaseInfo);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }

        public Task<Result<ScheduleDeleteInfo>> DeleteCouponAsync(int packId)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<ScheduleDeleteInfo>> DeletePackAsync(int packId)
        {
            var result = new Result<ScheduleDeleteInfo>();
            try
            {
                var exists = await packReadRepository.FirstOrDefaultAsync(new PackSpec(packId));
                if (exists == null)
                    return result.NotFound("");
                exists.IsActive = false;
                await packWriteRepository.UpdateAsync(exists);
                return result.Success(new ScheduleDeleteInfo
                {
                    Deleted = true
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, packId);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }

        public async Task<Result<PackInfo>> FindByIdAsync(int packId)
        {
            var result = new Result<PackInfo>();
            try
            {
                if (packId <= 0)
                    return result.Invalid(new List<ValidationError>
                    {
                        new ValidationError()
                        {
                            Identifier = $"Pack::{nameof(FindByIdAsync)}",
                            ErrorMessage = "PackId invalid",
                            Severity = ValidationSeverity.Warning
                        }
                    });

                var pack = await packReadRepository.FirstOrDefaultAsync(new PackSpec(packId), Utils.Commons.GetCancellationToken(15).Token);
                if (pack == null)
                    return result.NotFound($"Pack not exist: {packId}");

                return result.Success(mapper.Map<PackInfo>(pack));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, packId);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }

        public async Task<Result<IEnumerable<PackInfo>>> GetAllAsync()
        {
            var result = new Result<IEnumerable<PackInfo>>();
            try
            {
                var list = await packReadRepository.ListAsync();
                if (list == null)
                    return result.NotFound("");

                //list.Where(x => x.IsActive == true);
                return result.Success(list.Select(x => mapper.Map<PackInfo>(x)).Where(x => x.IsActive == true));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }

        public async Task<Result<PackInfo>> UpdatePackAsync(PackBaseInfo packBaseInfo, int id)
        {
            var result = new Result<PackInfo>();
            try
            {
                if (id < 1 || packBaseInfo == null)
                    return result.Invalid(new List<ValidationError>
                    {
                        new ValidationError()
                        {
                            Identifier = $"Course::{nameof(UpdatePackAsync)}",
                            ErrorMessage = "Invalid parameters",
                            Severity = ValidationSeverity.Warning
                        }
                    });
                var exists = await packReadRepository.FirstOrDefaultAsync(new PackSpec(id));
                if (exists == null)
                    return result.NotFound("");
                exists.Id = id;
                exists.Hours = packBaseInfo.Hours;
                exists.Savings = packBaseInfo.Savings;
                exists.Minutes = packBaseInfo.Minutes;
                exists.Price = packBaseInfo.Price;
                await packWriteRepository.UpdateAsync(exists);
                return result.Success(mapper.Map<PackInfo>(exists));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, id);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }
    }
}
