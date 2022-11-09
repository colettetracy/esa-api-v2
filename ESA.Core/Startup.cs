using ESA.Core.Interfaces;
using ESA.Core.Mapper;
using ESA.Core.Middleware;
using ESA.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ESA.Core
{
    public static class Startup
    {
        public static void AddBusinessLogic(this IServiceCollection services)
        {
            services.AddAutoMapper(config => config.AddProfile<MappingProfile>(), AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<Jwt>();
            services.AddScoped(typeof(IAuthService), typeof(AuthService));
            services.AddScoped(typeof(IAccountService), typeof(AccountService));
            services.AddScoped(typeof(IAccountProfileService), typeof(AccountProfileService));
            services.AddScoped(typeof(IAccountSurveyService), typeof(AccountSurveyService));
            
            services.AddScoped(typeof(ICourseService), typeof(CourseService));
            services.AddScoped(typeof(ICalendarService), typeof(CourseCalendarService));
            services.AddScoped(typeof(IScheduleService), typeof(CourseScheduleService));
            services.AddScoped(typeof(IReviewService), typeof(CourseReviewService));
            services.AddScoped(typeof(ICouponService), typeof(CouponService));

            services.AddScoped(typeof(IPackService), typeof(PackService));

            services.AddScoped(typeof(IDashboardService), typeof(DashboardService));

            services.AddScoped(typeof(IStudentService), typeof(StudentService));

            services.AddScoped(typeof(INotificationService), typeof(NotificationService));
        }
    }
}
