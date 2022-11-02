using Ardalis.Specification;


namespace ESA.Core.Specs
{
    public class PaymentSpec : Specification<Payment>
    {
        public PaymentSpec(string payCode)
        {
            Query.Where(x => x.PayCode == payCode).AsNoTracking();
        }
    }
}
