using ESA.Core.Models.Course;
using ESA.Core.Specs.Filters;
using GV.DomainModel.SharedKernel.Interop;

namespace ESA.Core.Interfaces
{
    public interface ICourseService
    {
        Task<Result<IEnumerable<CourseInfo>>> GetAllAsync();

        Task<Result<IEnumerable<CourseInfo>>> FilterAsync(CourseFilter filter);

        Task<Result<CourseInfo>> AddAsync(CourseBaseInfo courseInfo);

        Task<Result<CourseInfo>> UpdateAsync(int courseId, CourseBaseInfo courseInfo);
    }

    public interface ICalendarService
    {
        Task<Result<IEnumerable<CalendarInfo>>> GetAllAsync();

        Task<Result<IEnumerable<CalendarInfo>>> FindByCourseAsync(int courseId);

        Task<Result<IEnumerable<CalendarInfo>>> FindByTeacherAsync(int teacherId);

        Task<Result<CalendarInfo>> AddCalendarAsync(CalendarBaseInfo calendarInfo);
    }

    public interface IScheduleService
    {
        Task<Result<IEnumerable<ScheduleInfo>>> FilterAsync(ScheduleFilter filter);

        Task<Result<ScheduleInfo>> AddScheduleAsync(ScheduleBaseInfo scheduleInfo);
    }

    public interface IReviewService
    {
        Task<Result<IEnumerable<ReviewInfo>>> FilterAsync(ReviewFilter filter);

        Task<Result<ReviewInfo>> AddReviewAsync(ReviewBaseInfo reviewInfo);
    }
}
