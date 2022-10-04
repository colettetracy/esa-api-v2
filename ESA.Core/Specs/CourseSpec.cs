using Ardalis.Specification;
using ESA.Core.Entities;
using ESA.Core.Specs.Filters;

namespace ESA.Core.Specs
{
    public class CourseSpec : Specification<Course>
    {
        public CourseSpec(CourseFilter filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Code))
                Query.Where(x => x.Code == filter.Code);

            if (!string.IsNullOrWhiteSpace(filter.Title))
                Query.Where(x => x.Title == filter.Title);

            if (filter.InitialPrice > 0 && filter.FinalPrice > 0)
                Query.Where(x => x.Price >= filter.InitialPrice && x.Price <= filter.FinalPrice);
        }
    }
}
