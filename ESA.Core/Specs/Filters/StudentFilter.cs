namespace ESA.Core.Specs.Filters
{
    public class StudentFilter
    {
        public int ScheduleId { get; set; }

        public int StudentId { get; set; }

        public bool? PaymentConfirmed { get; set; }

        public bool? InvoiceConfirmed { get; set; }
    }

    public class StudentGroupFilter
    {
        public int CalendarId { get; set; }

        public int StudentId { get; set; }

        public bool? PaymentConfirmed { get; set; }

        public bool? InvoiceConfirmed { get; set; }
    }
}
