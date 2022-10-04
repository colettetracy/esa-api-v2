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
    }

    public class CalendarBaseInfo
    {
        [Required]
        public int CourseId { get; set; }

        [Required]
        public int TeacherId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime FinishDate { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
