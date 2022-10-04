namespace ESA.Core.Models.Student
{
    public class StudentGroupInfo
    {
        public int Id { get; set; }

        public int CalendarId { get; set; }

        public int StudentId { get; set; }

        public decimal Progress { get; set; }

        public bool? IsActive { get; set; }

        public bool IsCancelled { get; set; }

        public bool? PaymentConfirmed { get; set; }

        public bool InvoiceConfirmed { get; set; }

        public DateTime EnrolledDate { get; set; }

        public DateTime LastUpdate { get; set; }
    }

    public class StudentGroupCreate
    {
        public int CalendarId { get; set; }

        public int StudentId { get; set; }

        public decimal Progress { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsCancelled { get; set; } = false;

        public bool PaymentConfirmed { get; set; }

        public bool InvoiceConfirmed { get; set; } = false;
    }
}
