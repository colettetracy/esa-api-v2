using ESA.Core.Models.Notification;
using GV.DomainModel.SharedKernel.Interop;

namespace ESA.Core.Interfaces
{
    public interface INotificationService
    {
        Task<Result<MailResultBase<string>>> AccountDeleteNotification(int accountId);


        Task<Result<MailResultBase<string>>> PurchaseNotification(int scheduleId, int studentId);
    }
}
