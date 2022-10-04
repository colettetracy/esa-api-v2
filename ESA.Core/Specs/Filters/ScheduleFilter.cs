namespace ESA.Core.Specs.Filters
{
    public class ScheduleFilter
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public int TeacherId { get; set; }

        public DateTime? Schedule { get; set; }
    }
}
