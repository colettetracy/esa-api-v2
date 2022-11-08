using ESA.Core.Models.Account;

namespace ESA.Core.Models.Course
{
    public class ScheduleInfo
    {
        public int Id { get; set; }

        public int CalendarId { get; set; }

        public bool IsPrivate { get; set; }

        public short StudentsAllowed { get; set; }

        public int Minutes { get; set; }

        public DateTime Schedule { get; set; }

        public bool IsActive { get; set; }

        public DateTime LastUpdate { get; set; }

        public CourseInfo? Course { get; set; }

        public AccountInfo? Teacher { get; set; }
    }

    public class ScheduleBaseInfo
    {
        public int CalendarId { get; set; }

        public bool IsPrivate { get; set; }

        public short StudentsAllowed { get; set; }

        public int Minutes { get; set; }

        public DateTime Schedule { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
    }

    public class ScheduleDeleteInfo
    {
        public bool Deleted { get; set; }
    }

    public class AvailabilityInfo
    {
        public ScheduleInfo scheduleInfo { get; set; }
        public DateTime? Date { get; set; }
        public List<DateTime> Times { get; set; } = new List<DateTime>();
    }
}
