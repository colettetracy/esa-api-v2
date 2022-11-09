using Ardalis.Specification;

using ESA.Core.Specs.Filters;

namespace ESA.Core.Specs
{
    public class ReviewSpec : Specification<CourseReview>
    {
        public ReviewSpec(ReviewFilter filter)
        {
            Query.Include(x => x.CourseCalendar).ThenInclude(x=>x.Course);
            Query.Include(z=>z.CourseStudent).ThenInclude(w=>w.Student);
            if (filter.CourseId > 0)
                Query.Where(x => x.CourseCalendar.CourseId == filter.CourseId);

            if (filter.TeacherId > 0)
                Query.Where(x => x.CourseCalendar.TeacherId == filter.TeacherId);

            if (filter.CourseStudentId > 0)
                Query.Where(x => x.CourseStudentId == filter.CourseStudentId);

            Query.AsNoTracking();
        }
    }
}
