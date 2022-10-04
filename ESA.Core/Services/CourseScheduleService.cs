using AutoMapper;
using ESA.Core.Entities;
using ESA.Core.Interfaces;
using ESA.Core.Models.Course;
using ESA.Core.Specs;
using ESA.Core.Specs.Filters;
using ESA.Core.Validators;
using GV.DomainModel.SharedKernel.Extensions;
using GV.DomainModel.SharedKernel.Interfaces;
using GV.DomainModel.SharedKernel.Interop;

namespace ESA.Core.Services
{
    public class CourseScheduleService : IScheduleService
    {
        private readonly IMapper mapper;
        private readonly IAppLogger<CourseScheduleService> logger;
        private readonly IRepository<CourseSchedule> scheduleWriteRepository;
        private readonly IReadRepository<CourseSchedule> scheduleReadRepository;

        public CourseScheduleService(
            IMapper mapper, 
            IAppLogger<CourseScheduleService> logger, 
            IRepository<CourseSchedule> scheduleWriteRepository, 
            IReadRepository<CourseSchedule> scheduleReadRepository)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.scheduleWriteRepository = scheduleWriteRepository;
            this.scheduleReadRepository = scheduleReadRepository;
        }

        public async Task<Result<ScheduleInfo>> AddScheduleAsync(ScheduleBaseInfo scheduleInfo)
        {
            var result = new Result<ScheduleInfo>();
            try
            {
                if (scheduleInfo == null)
                    return result.Invalid(new List<ValidationError>
                    {
                        new ValidationError()
                        {
                            Identifier = $"Schedule::{AddScheduleAsync}",
                            ErrorMessage = "Model invalid",
                            Severity = ValidationSeverity.Warning
                        }
                    });

                var validatorInfo = new ScheduleValidator();
                var validationInfo = validatorInfo.Validate(scheduleInfo);
                if (!validationInfo.IsValid)
                    return result.Invalid(validationInfo.AsErrors());

                var schedule = mapper.Map<CourseSchedule>(scheduleInfo);
                if (schedule == null)
                    return result.Conflict("Mapping error");

                schedule.IsActive = true;
                schedule.LastUpdate = DateTime.UtcNow;
                schedule = await scheduleWriteRepository.AddAsync(schedule);
                if (schedule == null)
                    return result.Conflict("Saving error");

                return result.Success(mapper.Map<ScheduleInfo>(schedule));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, scheduleInfo);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }

        public async Task<Result<IEnumerable<ScheduleInfo>>> FilterAsync(ScheduleFilter filter)
        {
            var result = new Result<IEnumerable<ScheduleInfo>>();
            try
            {
                var list = await scheduleReadRepository.ListAsync(new ScheduleSpec(filter));
                if (list == null)
                    return result.NotFound("");

                return result.Success(list.Select(x => mapper.Map<ScheduleInfo>(x)));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, filter);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }
    }
}
