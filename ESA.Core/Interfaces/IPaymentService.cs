using ESA.Core.Models.Payment;
using GV.DomainModel.SharedKernel.Interop;

namespace ESA.Core.Interfaces
{
    public interface IPaymentService
    {
        Task<Result<PaymentInfo>> AddPayFromPayPalAsync(PayPalInfo paypal);
    }
}
