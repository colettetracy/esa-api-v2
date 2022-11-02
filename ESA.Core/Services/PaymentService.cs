using AutoMapper;

using ESA.Core.Interfaces;
using ESA.Core.Models.Payment;
using ESA.Core.Specs;
using GV.DomainModel.SharedKernel.Interfaces;
using GV.DomainModel.SharedKernel.Interop;

namespace ESA.Core.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IMapper mapper;
        private readonly IAppLogger<PaymentService> logger;
        private readonly IRepository<Payment> paymentWriteRepository;
        private readonly IReadRepository<Payment> paymentReadRepository;

        public PaymentService(
            IMapper mapper, 
            IAppLogger<PaymentService> logger, 
            IRepository<Payment> paymentWriteRepository, 
            IReadRepository<Payment> paymentReadRepository)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.paymentWriteRepository = paymentWriteRepository;
            this.paymentReadRepository = paymentReadRepository;
        }

        public async Task<Result<PaymentInfo>> AddPayFromPayPalAsync(PayPalInfo paypal)
        {
            var result = new Result<PaymentInfo>();
            try
            {
                if (paypal == null)
                    return result.Invalid(new List<ValidationError>
                    {
                        new ValidationError()
                        {
                            Identifier = $"Payment::{AddPayFromPayPalAsync}",
                            ErrorMessage = "Model invalid",
                            Severity = ValidationSeverity.Warning
                        }
                    });

                var payment = await paymentReadRepository.FirstOrDefaultAsync(new PaymentSpec(paypal.Id));
                if (payment != null)
                    return result.Conflict($"Payment already exists: {paypal.Id}");

                payment = mapper.Map<Payment>(paypal);
                if (payment == null)
                    return result.Conflict("Mapping error");

                payment.PaymentMethodId = 1;
                payment.PaymentStatusId = (short)(paypal.Status.Equals("COMPLETED") ? 1 : 2);
                payment.CreationDate = paypal.CreateTime.UtcDateTime;
                payment.LastUpdate = DateTime.UtcNow;
                payment = await paymentWriteRepository.AddAsync(payment);
                if (payment == null)
                    return result.Conflict("Saving error");

                return result.Success(new PaymentInfo
                {
                    Id = paypal.Id,
                    LastUpdate = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, paypal);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }
    }
}
