using Ardalis.Specification;
using ESA.Core.Entities;

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
            Query.Where(x => x.CourseId == courseId);
        }
    }
}
