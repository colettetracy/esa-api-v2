using ESA.Core.Models.Account;
using System.ComponentModel.DataAnnotations;

namespace ESA.Core.Models.Course
{
    public class CalendarInfo
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public int TeacherId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        public bool IsActive { get; set; }

        public DateTime LastUpdate { get; set; }
        public AccountInfo Teacher { get; set; }
        public List<ScheduleInfo> CourseSchedule { get; set; } = new List<ScheduleInfo>();
    }

    public class CalendarBaseInfo
    {
        
        public int CourseId { get; set; }

        
        public int TeacherId { get; set; }

        
        public DateTime StartDate { get; set; }

        
        public DateTime FinishDate { get; set; }

        public bool IsActive { get; set; } = true;

        public List<ScheduleBaseInfo> CourseSchedules { set; get; } = new List<ScheduleBaseInfo>();
    }
}
