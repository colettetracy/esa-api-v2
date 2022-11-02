using ESA.Core.Interfaces;
using GV.DomainModel.SharedKernel.Interfaces;
using GV.Libraries.NotificationServices.Email;
using GV.Libraries.NotificationServices;
using Microsoft.AspNetCore.Http;
using GV.DomainModel.SharedKernel.Interop;
using ESA.Core.Models.Notification;
using ESA.Core.Specs;

namespace ESA.Core.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IEmailService emailService;
        private readonly IAppLogger<NotificationService> logger;
        private readonly IReadRepository<CourseSchedule> scheduleRepository;
        private readonly IReadRepository<CourseStudent> studentRepository;
        private readonly IReadRepository<Account> accountRepository;

        public NotificationService(
            IEmailService emailService,
            IAppLogger<NotificationService> logger,
            IReadRepository<CourseSchedule> scheduleRepository,
            IReadRepository<CourseStudent> studentRepository,
            IReadRepository<Account> accountRepository)
        {
            this.logger = logger;
            this.emailService = emailService;
            this.scheduleRepository = scheduleRepository;
            this.studentRepository = studentRepository;
            this.accountRepository = accountRepository;
        }

        public async Task<Result<MailResultBase<string>>> AccountDeleteNotification(int accountId)
        {
            var result = new Result<MailResultBase<string>>();
            try
            {
                var account = await accountRepository.GetByIdAsync(accountId);
                if (account == null)
                    return result.NotFound("Account not exists");

                if (account.IsActive)
                    return result.Conflict("Account is active");

                string body = "We are sorry that you are no longer part of our academy and we wish you the best of success.\n" +
                    "We hope you come back soon.";

                string htmlTemplate = GetEmailTemplate("AccountDeleteNotification.html")
                    .Replace("[%STUDENT_NAME%]", $"{account?.FirstName} {account?.LastName}")
                    .Replace("[%CONTENT%]", body)
                    .Replace("[%YEAR%]", DateTime.Now.Year.ToString());

                var contact = new Contact
                {
                    Email = account?.Email.ToLower(),
                    Name = $"{account?.FirstName} {account?.LastName}"
                };
                var request = new SendEmailRequest
                {
                    To = new Contact[] { contact },
                    From = new Contact { Email = "admin@emmanuelspanishacademy.com", Name = "Emmanuel Spanish Academy" },
                    Body = htmlTemplate,
                    BodyIsHtml = true,
                    Subject = "Account delete"
                };
                bool sent = await emailService.SendAsync(request);

                return result.Success(new MailResultBase<string>
                {
                    Info = sent ? $"Mail send to {contact.Email}" : "Fail send mail",
                    MailStatusCode = sent ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, accountId);
                return result.Error("An unexpected error has occurred::SendMail", ex.Message);
            }
        }

        public async Task<Result<MailResultBase<string>>> PurchaseNotification(int scheduleId, int studentId)
        {
            var result = new Result<MailResultBase<string>>();
            try
            {
                var schedule = await scheduleRepository.FirstOrDefaultAsync(new ScheduleSpec(scheduleId));
                var student = await studentRepository.FirstOrDefaultAsync(new StudentSpec(scheduleId, studentId));
                var teacher = $"{schedule?.CourseCalendar.Teacher.FirstName} {schedule?.CourseCalendar.Teacher.LastName}";

                var studentResult = StudentEmail(schedule, student, teacher);
                var request1 = studentResult.Request;

                var teacherResult = TeacherEmail(schedule, student);
                var request2 = teacherResult.Request;

                bool sent = await emailService.SendAsync(request1);
                bool sent2 = await emailService.SendAsync(request2);
                var contact1 = studentResult.Contact;

                return result.Success(new MailResultBase<string>
                {
                    Info = sent ? $"Mail send to {contact1.Email}" : "Fail send mail",
                    MailStatusCode = sent ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, scheduleId, studentId);
                return result.Error("An unexpected error has occurred::SendMail", ex.Message);
            }
        }

        private static ResultEmail StudentEmail(CourseSchedule schedule, CourseStudent student, string teacher)
        {
            string body = $"Your lesson with {teacher} has been confirmed for {student.EnrolledDate.ToShortDateString()} at {student.EnrolledDate.ToShortTimeString()}.\r\n" +
                    $"This lesson will be delivered using the {teacher} Zoom room. Click here to access {schedule?.CourseCalendar.Teacher.AccountProfile.ZoomLink}";

            string htmlTemplate = GetEmailTemplate("PurchaseNotification.html")
                .Replace("[%STUDENT_NAME%]", $"{student?.Student.FirstName} {student?.Student.LastName}")
                .Replace("[%CONTENT%]", body)
                .Replace("[%ZOOM_LINK%]", schedule?.CourseCalendar.Teacher.AccountProfile.ZoomLink)
                .Replace("[%YEAR%]", DateTime.Now.Year.ToString());

            var contact = new Contact
            {
                Email = student?.Student.Email.ToLower(),
                Name = $"{student?.Student.FirstName} {student?.Student.LastName}"
            };
            var request = new SendEmailRequest
            {
                To = new Contact[] { contact },
                From = new Contact { Email = "admin@emmanuelspanishacademy.com", Name = "Emmanuel Spanish Academy" },
                Body = htmlTemplate,
                BodyIsHtml = true,
                Subject = "Purchase notification"
            };
            return new ResultEmail { Request = request, Contact = contact };
        }

        private static ResultEmail TeacherEmail(CourseSchedule schedule, CourseStudent student)
        {
            string body = $"{student.Student.FirstName} {student.Student.LastName} has scheduled a lesson with you on {student.EnrolledDate.ToShortDateString()} at {student.EnrolledDate.ToShortTimeString()}.\r\n" +
                    $"Please remember to use your assigned Zoom room to join the session. Thank you!";

            string htmlTemplate = GetEmailTemplate("TeacherPurchaseNotification.html")
                .Replace("[%STUDENT_NAME%]", $"{schedule?.CourseCalendar.Teacher.FirstName} {schedule?.CourseCalendar.Teacher.LastName}")
                .Replace("[%CONTENT%]", body)
                .Replace("[%ZOOM_LINK%]", schedule?.CourseCalendar.Teacher.AccountProfile.ZoomLink)
                .Replace("[%YEAR%]", DateTime.Now.Year.ToString());

            var contact = new Contact
            {
                Email = schedule?.CourseCalendar?.Teacher.Email.ToLower(),
                Name = $"{schedule?.CourseCalendar?.Teacher.FirstName} {schedule?.CourseCalendar?.Teacher.LastName}"
            };
            var adminContact = new Contact { Email = "admin@emmanuelspanishacademy.com", Name = "Emmanuel Spanish Academy" };
            var request = new SendEmailRequest
            {
                To = new Contact[] { contact, adminContact },
                From = adminContact,
                Body = htmlTemplate,
                BodyIsHtml = true,
                Subject = "New lesson scheduled"
            };
            return new ResultEmail { Request = request, Contact = contact };
        }

        private static string GetEmailTemplate(string fileName)
        {
            var enviroment = Environment.CurrentDirectory;
            var filePath = Path.Combine(enviroment, "bin\\Development\\net6.0\\", "EmailTemplates", fileName);
            if (!File.Exists(filePath))
            {
                filePath = Path.Combine(enviroment, "EmailTemplates", fileName);
                if (!File.Exists(filePath))
                    throw new FileNotFoundException(filePath);
            }

            return File.ReadAllText(filePath);
        }
    }

    class ResultEmail
    {
        public SendEmailRequest Request { get; set; }
        public Contact Contact { get; set; }
    }
}
