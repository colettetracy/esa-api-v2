using System.ComponentModel.DataAnnotations;

namespace ESA.Core.Models.Course
{
    public class CourseInfo
    {
        public int Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Subtitle { get; set; } = string.Empty;

        public string About { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public short DurationDays { get; set; }

        public string CurrencyCode { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string Icon { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public DateTime LastUpdate { get; set; }
    }

    public class CourseBaseInfo
    {
        [Required]
        public string Code { get; set; } = string.Empty;

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Subtitle { get; set; } = string.Empty;

        [Required]
        public string About { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public short DurationDays { get; set; }

        [Required]
        public string CurrencyCode { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Icon { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}
