using Ardalis.Specification;


namespace ESA.Core.Specs
{
    public class CalendarSpec : Specification<CourseCalendar>
    {
        public CalendarSpec(int teacherId)
        {
            Query.Where(x => x.TeacherId == teacherId);
        }

        public CalendarSpec(int courseId, bool isActive)
        {
            Query.Include(x => x.CourseSchedule);
            Query.Include(x => x.Teacher);
            Query.Where(x => x.CourseId == courseId);
        }
    }
}
