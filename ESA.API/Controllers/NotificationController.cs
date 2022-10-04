using ESA.Core.Interfaces;
using ESA.Core.Models.Notification;
using GV.DomainModel.SharedKernel.Interop;
using Microsoft.AspNetCore.Mvc;

namespace ESA.API.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class NotificationController : ApiControllerBase
    {
        private readonly INotificationService notificationService;

        public NotificationController(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        [HttpPost("account-delete/{accountId}")]
        [ProducesResponseType(typeof(MailResultBase<string>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<MailResultBase<string>>>> AccountDeleteNotificationAsync([FromRoute] int accountId)
        {
            return await notificationService.AccountDeleteNotification(accountId);
        }

        [HttpPost("purchase")]
        [ProducesResponseType(typeof(MailResultBase<string>), StatusCodes.Status200OK)]
        public async Task<ActionResult<Result<MailResultBase<string>>>> SendPurchaseNotificationAsync([FromBody] MailPurchase mailPurchase)
        {
            return await notificationService.PurchaseNotification(mailPurchase.ScheduleId, mailPurchase.StudentId);
        }
    }
}
