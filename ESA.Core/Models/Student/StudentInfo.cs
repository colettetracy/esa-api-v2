﻿namespace ESA.Core.Models.Student
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

        public DateTime EnrolledDate { get; set; }

        public DateTime LastUpdate { get; set; }

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

        public DateTime EnrolledDate { get; set; }// = DateTime.UtcNow;

        public List<FriendBaseInfo> Friends { get; set; } = new List<FriendBaseInfo>();
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
