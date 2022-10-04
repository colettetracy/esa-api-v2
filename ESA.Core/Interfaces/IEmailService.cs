using GV.Libraries.NotificationServices.Email;

namespace ESA.Core.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendAsync(SendEmailRequest message);
    }
}
