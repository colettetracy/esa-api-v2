using AutoMapper;

using ESA.Core.Interfaces;
using ESA.Core.Models.Course;
using ESA.Core.Specs;
using ESA.Core.Validators;
using GV.DomainModel.SharedKernel.Extensions;
using GV.DomainModel.SharedKernel.Interfaces;
using GV.DomainModel.SharedKernel.Interop;

namespace ESA.Core.Services
{
    public class CourseCalendarService : ICalendarService
    {
        private readonly IMapper mapper;
        private readonly IAppLogger<CourseCalendarService> logger;
        private readonly IRepository<CourseCalendar> calendarWriteRepository;
        private readonly IReadRepository<CourseCalendar> calendarReadRepository;

        public CourseCalendarService(
            IMapper mapper,
            IAppLogger<CourseCalendarService> logger,
            IRepository<CourseCalendar> calendarWriteRepository,
            IReadRepository<CourseCalendar> calendarReadRepository)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.calendarWriteRepository = calendarWriteRepository;
            this.calendarReadRepository = calendarReadRepository;
        }

        public async Task<Result<CalendarInfo>> AddCalendarAsync(List<CalendarBaseInfo> calendarInfo)
        {
            var result = new Result<CalendarInfo>();
            try
            {
                if (calendarInfo == null)
                    return result.Invalid(new List<ValidationError>
                    {
                        new ValidationError()
                        {
                            Identifier = $"Calendar::{AddCalendarAsync}",
                            ErrorMessage = "Model invalid",
                            Severity = ValidationSeverity.Warning
                        }
                    });

                CalendarInfo res = new();
                foreach (var ci in calendarInfo)
                {
                    var calendar = mapper.Map<CourseCalendar>(ci);
                    if (calendar == null)
                        return result.Conflict("Mapping error");

                    calendar.IsActive = true;
                    calendar.LastUpdate = DateTime.UtcNow;
                    foreach(var sch in ci.CourseSchedules)
                    {
                        calendar.CourseSchedule.Add(mapper.Map<CourseSchedule>(sch));
                    }
                    calendar = await calendarWriteRepository.AddAsync(calendar, Utils.Commons.GetCancellationToken(60).Token);
                    if (calendar == null)
                        return result.Conflict("Save error");

                    res = mapper.Map<CalendarInfo>(calendar);

                }
                if (res == null)
                    return result.Conflict("Save error");
                return result.Success(res);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, calendarInfo);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }

        public async Task<Result<IEnumerable<CalendarInfo>>> FindByCourseAsync(int courseId)
        {
            var result = new Result<IEnumerable<CalendarInfo>>();
            try
            {
                if (courseId <= 0)
                    return result.Invalid(new List<ValidationError>
                    {
                        new ValidationError()
                        {
                            Identifier = $"Calendar::{nameof(FindByCourseAsync)}",
                            ErrorMessage = "CourseId invalid",
                            Severity = ValidationSeverity.Warning
                        }
                    });

                var list = await calendarReadRepository.ListAsync(new CalendarSpec(courseId, true));
                if (list == null)
                    return result.NotFound("");

                return result.Success(list.Select(x => mapper.Map<CalendarInfo>(x)));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }

        public async Task<Result<IEnumerable<CalendarInfo>>> FindByTeacherAsync(int teacherId)
        {
            var result = new Result<IEnumerable<CalendarInfo>>();
            try
            {
                if (teacherId <= 0)
                    return result.Invalid(new List<ValidationError>
                    {
                        new ValidationError()
                        {
                            Identifier = $"Calendar::{nameof(FindByTeacherAsync)}",
                            ErrorMessage = "TeacherId invalid",
                            Severity = ValidationSeverity.Warning
                        }
                    });

                var list = await calendarReadRepository.ListAsync(new CalendarSpec(teacherId));
                if (list == null)
                    return result.NotFound("");

                return result.Success(list.Select(x => mapper.Map<CalendarInfo>(x)));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }

        public async Task<Result<IEnumerable<CalendarInfo>>> GetAllAsync()
        {
            var result = new Result<IEnumerable<CalendarInfo>>();
            try
            {
                var list = await calendarReadRepository.ListAsync();
                if (list == null)
                    return result.NotFound("");

                return result.Success(list.Select(x => mapper.Map<CalendarInfo>(x)));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }
    }
}
