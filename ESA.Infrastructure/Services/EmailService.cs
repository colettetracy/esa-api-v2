using ESA.Core.Interfaces;
using GV.DomainModel.SharedKernel.Interfaces;
using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;

namespace ESA.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IAppLogger<EmailService> logger;
        private readonly IMailjetClient mailjetClient;

        public EmailService(IAppLogger<EmailService> logger, IMailjetClient mailjetClient)
        {
            this.logger = logger;
            this.mailjetClient = mailjetClient;
        }

        public async Task<bool> SendAsync(GV.Libraries.NotificationServices.Email.SendEmailRequest message)
        {
            try
            {
                logger.LogInformation("SendMail ==> Preparing");
                #region Validation
                if (message == null)
                    throw new System.ArgumentNullException(nameof(GV.Libraries.NotificationServices.Email.SendEmailRequest), "Invalid");

                if (message.From == null)
                    throw new System.ArgumentNullException(nameof(message.From), "Invalid");

                if (message.To == null || !message.To.Any())
                    throw new System.ArgumentNullException(nameof(message.To), "Invalid");

                if (string.IsNullOrWhiteSpace(message.Subject))
                    throw new System.ArgumentNullException(nameof(message.Subject), "Invalid");

                if (string.IsNullOrWhiteSpace(message.Body))
                    throw new System.ArgumentNullException(nameof(message.Body), "Invalid");
                #endregion

                var mail = new TransactionalEmail
                {
                    From = new SendContact(message.From.Email, message.From.Name),
                    To = message.To.Select(x => new SendContact(x.Email, x.Name)).ToList(),
                    Subject = message.Subject,
                    TrackOpens = TrackOpens.enabled,
                    TrackClicks = TrackClicks.enabled
                };
                #region CC/BCC
                if (message.Copy != null)
                    mail.Cc = message.Copy.Select(x => new SendContact(x.Email, x.Name)).ToList();

                if (message.HiddenCopy != null)
                    mail.Bcc = message.HiddenCopy.Select(x => new SendContact(x.Email, x.Name)).ToList();
                #endregion
                #region MailBody
                if (message.BodyIsHtml.GetValueOrDefault())
                    mail.HTMLPart = message.Body;
                else mail.TextPart = message.Body;

                if (message.Attachments != null)
                    mail.Attachments = message.Attachments
                        .Select(at => new Attachment(
                            Guid.NewGuid().ToString(),
                            at.MimeType,
                            Convert.ToBase64String(at.Data))
                        ).ToList();
                #endregion

                logger.LogInformation("SendMail ==> Sending");
                var response = await mailjetClient.SendTransactionalEmailAsync(mail, false, true);
                logger.LogInformation($"SendMail ==> Messages: {response.Messages?.Length}");

                #region Decision
                if (response.Messages != null)
                {
                    var result = response.Messages?.Select(res => new MailResponse(
                        res.Errors != null && res.Errors.Any(),
                        res.Errors == null || !res.Errors.Any())).First();
                    if (result == null)
                        return false;

                    if (!result.Successful && result.Errors)
                    {
                        logger.LogError(null, "SendMail ==> All fails", response);
                        return false;
                    }
                    if (result.Successful && result.Errors)
                        logger.LogWarning("SendMail ==> Some failed", response);
                }
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "failed to send mail", message);
                return false;
            }
        }
    }

    public class MailResponse
    {
        public bool Errors { get; private set; }

        public bool Successful { get; private set; }

        public MailResponse(bool errors, bool successful)
        {
            Errors = errors;
            Successful = successful;
        }
    }
}
