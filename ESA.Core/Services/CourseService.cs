using AutoMapper;

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
    public class CourseService : ICourseService
    {
        private readonly IMapper mapper;
        private readonly IAppLogger<CourseService> logger;
        private readonly IRepository<Course> courseWriteRepository;
        private readonly IReadRepository<Course> courseReadRepository;

        public CourseService(
            IMapper mapper, 
            IAppLogger<CourseService> logger, 
            IRepository<Course> courseWriteRepository, 
            IReadRepository<Course> courseReadRepository)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.courseWriteRepository = courseWriteRepository;
            this.courseReadRepository = courseReadRepository;
        }

        public async Task<Result<CourseInfo>> AddAsync(CourseBaseInfo courseInfo)
        {
            var result = new Result<CourseInfo>();
            try
            {
                if (courseInfo == null)
                    return result.Invalid(new List<ValidationError>
                    {
                        new ValidationError()
                        {
                            Identifier = $"Course::{nameof(AddAsync)}",
                            ErrorMessage = "Model invalid",
                            Severity = ValidationSeverity.Warning
                        }
                    });

                var validatorInfo = new CourseValidator();
                var validationInfo = validatorInfo.Validate(courseInfo);
                if (!validationInfo.IsValid)
                    return result.Invalid(validationInfo.AsErrors());

                var course = await courseWriteRepository.AddAsync(mapper.Map<Course>(courseInfo));
                if (course == null)
                    return result.Conflict("Save error");

                return result.Success(mapper.Map<CourseInfo>(course));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, courseInfo);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }

        public async Task<Result<IEnumerable<CourseInfo>>> FilterAsync(CourseFilter filter)
        {
            var result = new Result<IEnumerable<CourseInfo>>();
            try
            {
                if (filter == null)
                    return result.Invalid(new List<ValidationError>
                    {
                        new ValidationError()
                        {
                            Identifier = $"Course::{nameof(FilterAsync)}",
                            ErrorMessage = "Model invalid",
                            Severity = ValidationSeverity.Warning
                        }
                    });

                var validatorInfo = new CourseFilterValidator();
                var validationInfo = validatorInfo.Validate(filter);
                if (!validationInfo.IsValid)
                    return result.Invalid(validationInfo.AsErrors());

                var list = await courseReadRepository.ListAsync(new CourseSpec(filter));
                if (list == null)
                    return result.NotFound("");

                return result.Success(list.Select(x => mapper.Map<CourseInfo>(x)));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, filter);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }

        public async Task<Result<IEnumerable<CourseInfo>>> GetAllAsync()
        {
            var result = new Result<IEnumerable<CourseInfo>>();
            try
            {
                var list = await courseReadRepository.ListAsync();
                if (list == null)
                    return result.NotFound("");

                //list.Where(x => x.IsActive == true);
                return result.Success(list.Select(x => mapper.Map<CourseInfo>(x)).Where(x=>x.IsActive==true));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }

        public async Task<Result<ScheduleDeleteInfo>> DeleteAsync(int courseId)
        {
            var result = new Result<ScheduleDeleteInfo>();
            try
            {
                var exists = await courseReadRepository.FirstOrDefaultAsync(new CourseSpec(courseId));
                if (exists == null)
                    return result.NotFound("");
                exists.IsActive = false;
                await courseWriteRepository.UpdateAsync(exists);
                return result.Success(new ScheduleDeleteInfo
                {
                    Deleted = true
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, courseId);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }

        public async Task<Result<CourseInfo>> UpdateAsync(int courseId, CourseBaseInfo courseInfo)
        {
            var result = new Result<CourseInfo>();
            try
            {
                if (courseId < 1 || courseInfo == null)
                    return result.Invalid(new List<ValidationError>
                    {
                        new ValidationError()
                        {
                            Identifier = $"Course::{nameof(UpdateAsync)}",
                            ErrorMessage = "Invalid parameters",
                            Severity = ValidationSeverity.Warning
                        }
                    });

                var validatorInfo = new CourseValidator();
                var validationInfo = validatorInfo.Validate(courseInfo);
                if (!validationInfo.IsValid)
                    return result.Invalid(validationInfo.AsErrors());

                var course = await courseReadRepository.GetByIdAsync(courseId, Utils.Commons.GetCancellationToken(15).Token);
                if (course == null)
                    return result.NotFound($"Course not exist: {courseId}");

                course.About = courseInfo.About;
                course.Code = courseInfo.Code;
                course.Content = courseInfo.Content;
                course.CurrencyCode = courseInfo.CurrencyCode;
                course.DurationDays = courseInfo.DurationDays;
                if (!string.IsNullOrWhiteSpace(courseInfo.Icon))
                {
                    course.Icon = Convert.FromBase64String(courseInfo.Icon);
                }
                course.IsActive = courseInfo.IsActive;
                course.Price = courseInfo.Price;
                course.Subtitle = courseInfo.Subtitle;
                course.Title = courseInfo.Title;
                course.LastUpdate = DateTime.UtcNow;

                await courseWriteRepository.UpdateAsync(course);
                return result.Success(mapper.Map<CourseInfo>(course));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, courseInfo);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }
    }
}
