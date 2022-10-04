namespace ESA.Core.Models.Notification
{
    public class MailResultBase<T>
    {
        public T? Info { get; set; }

        public int MailStatusCode { get; set; }
    }
}
