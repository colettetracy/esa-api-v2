using ESA.Core.Models.Account;
using ESA.Core.Models.Course;

namespace ESA.Core.Models.Student
{
    public class StudentInfo
    {
        public int Id { get; set; }

        public int ScheduleId { get; set; }

        public int StudentId { get; set; }

        public decimal Progress { get; set; }

        public bool IsActive { get; set; }

        public bool IsCancelled { get; set; }

        public bool PaymentConfirmed { get; set; }

        public int TimePurchased { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }

        public DateTime EnrolledDate { get; set; }

        public DateTime LastUpdate { get; set; }

        public AccountInfo Student { get; set; }
        public ScheduleInfo CourseSchedule { get; set; }

        public IEnumerable<FriendInfo> Friends { get; set; } = Enumerable.Empty<FriendInfo>();
    }

    public class StudentBaseInfo
    {
        public int ScheduleId { get; set; }

        public int StudentId { get; set; }

        public decimal Progress { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public bool IsCancelled { get; set; } = false;

        public bool PaymentConfirmed { get; set; } = false;

        public int TimePurchased { get; set; }
        public decimal Amount { get; set; }

        public string? Coupon { get; set; }

        public DateTime EnrolledDate { get; set; }// = DateTime.UtcNow;

        public List<FriendBaseInfo> Friends { get; set; } = new List<FriendBaseInfo>();
    }
    public class StudentCouponBaseInfo
    {
        public int Id { set; get; }
        public string Coupon { get; set; } = String.Empty;

    }

    public class PaymentConfirmBaseInfo
    {
        public int Id { set; get; }

    }
    public class StudentDeleteBaseInfo
    {
        public int Id { set; get; }

    }
    public class FriendInfo
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime LastUpdate { get; set; }
    }

    public class FriendBaseInfo
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime LastUpdate { get; set; } = DateTime.UtcNow;
    }
}
