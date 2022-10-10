using Ardalis.Specification;
using ESA.Core.Entities;
using ESA.Core.Specs.Filters;

namespace ESA.Core.Specs
{
    public class StudentSpec : Specification<CourseStudent>
    {
        public StudentSpec(StudentFilter filter)
        {
            Query.Include(x => x.CourseSchedule).ThenInclude(x => x.CourseCalendar.Teacher)
                .Include(x => x.CourseSchedule).ThenInclude(x => x.CourseCalendar.Course)
                .Include(x => x.CourseStudentFriend)
                .Include(x => x.Student);

            if (filter.ScheduleId > 0)
                Query.Where(x => x.CourseScheduleId == filter.ScheduleId);

            if (filter.TeacherId > 0)
                Query.Where(x => x.CourseSchedule.CourseCalendar.TeacherId == filter.TeacherId);

            if (filter.StudentId > 0)
                Query.Where(x => x.StudentId == filter.StudentId);

            if (filter.PaymentConfirmed.HasValue)
                Query.Where(x => x.PaymentConfirmed == filter.PaymentConfirmed);

            Query.AsNoTracking();
        }

        public StudentSpec(int scheduleId, int studentId)
        {
            Query.Include(x => x.Student.AccountProfile)
                .Where(x => x.CourseScheduleId == scheduleId && x.StudentId == studentId)
                .AsNoTracking();
        }
    }

    public class StudentGroupSpec : Specification<CourseStudentGroup>
    {
        public StudentGroupSpec(StudentGroupFilter filter)
        {
            Query.Include(x => x.CourseCalendar)
                .Include(x => x.Student);

            if (filter.CalendarId > 0)
                Query.Where(x => x.CourseCalendarId == filter.CalendarId);

            if (filter.StudentId > 0)
                Query.Where(x => x.StudentId == filter.StudentId);

            if (filter.PaymentConfirmed.HasValue)
                Query.Where(x => x.PaymentConfirmed == filter.PaymentConfirmed);

            if (filter.InvoiceConfirmed.HasValue)
                Query.Where(x => x.InvoiceConfirmed == filter.InvoiceConfirmed);

            Query.AsNoTracking();
        }
    }
}
