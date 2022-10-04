using Ardalis.Specification;
using ESA.Core.Entities;
using ESA.Core.Specs.Filters;

namespace ESA.Core.Specs
{
    public class ScheduleSpec : Specification<CourseSchedule>
    {
        public ScheduleSpec(ScheduleFilter filter)
        {
            Query.Include(x => x.CourseCalendar.Course);
            Query.Include(x => x.CourseCalendar.Teacher);

            if (filter.Id > 0)
                Query.Where(x => x.Id == filter.Id);

            if (filter.CourseId > 0)
                Query.Where(x => x.CourseCalendar.CourseId == filter.CourseId);

            if (filter.TeacherId > 0)
                Query.Where(x => x.CourseCalendar.TeacherId == filter.TeacherId);

            if (filter.Schedule.HasValue)
                Query.Where(x => x.Schedule.Date == filter.Schedule.Value.Date);

            Query.AsNoTracking();
        }

        public ScheduleSpec(int scheduleId)
        {
            Query.Include(x => x.CourseCalendar.Course);
            Query.Include(x => x.CourseCalendar.Teacher.AccountProfile);

            if (scheduleId > 0)
                Query.Where(x => x.Id == scheduleId);

            Query.AsNoTracking();
        }
    }
}
