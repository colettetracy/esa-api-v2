using AutoMapper;
using ESA.Core.Entities;
using ESA.Core.Models.Account;
using ESA.Core.Models.Course;
using ESA.Core.Models.Payment;
using ESA.Core.Models.Student;

namespace ESA.Core.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            /// Account
            CreateMap<Account, AccountInfo>()
                .ForMember(dest => dest.ProfilePicture, act => act.MapFrom(src => src.ProfilePicture != null ? Convert.ToBase64String(src.ProfilePicture) : src.ProfilePicturePath))
                .ForMember(dest => dest.Profile, act => act.MapFrom(src => src.AccountProfile))
                .ReverseMap();
            CreateMap<Role, RoleInfo>().ReverseMap();
            CreateMap<AccountBaseInfo, Account>().ReverseMap();

            /// Profile
            CreateMap<AccountProfile, AccountProfileInfo>()
                .ForMember(dest => dest.DateOfBirth, act => act.MapFrom(src => src.DateOfBirth.GetValueOrDefault().ToDateTime(TimeOnly.MaxValue)))
                .ReverseMap();
            CreateMap<AccountProfileBase, AccountProfile>()
                .ForMember(dest => dest.DateOfBirth, act => act.MapFrom(src => DateOnly.FromDateTime(src.DateOfBirth)))
                .ReverseMap();

            /// Survey
            CreateMap<AccountSurvey, AccountSurveyInfo>().ReverseMap();
            CreateMap<AccountSurveyBaseInfo, AccountSurvey>().ReverseMap();

            /// Course
            CreateMap<Course, CourseInfo>()
                .ForMember(dest => dest.Icon, act => act.MapFrom(src => src.Icon != null ? Convert.ToBase64String(src.Icon) : null))
                .ReverseMap();
            CreateMap<Course, CourseBaseInfo>()
                .ForMember(dest => dest.Icon, act => act.MapFrom(src => src.Icon != null ? Convert.ToBase64String(src.Icon) : null))
                .ReverseMap();
            CreateMap<CourseBaseInfo, Course>()
               .ForMember(dest => dest.Icon, act => act.MapFrom(src => src.Icon != null ? Convert.FromBase64String(src.Icon) : null))
               .ReverseMap();

            /// Calendar
            CreateMap<CourseCalendar, CalendarInfo>()
                .ForMember(dest => dest.StartDate, act => act.MapFrom(src => src.StartDate.ToDateTime(TimeOnly.MinValue)))
                .ForMember(dest => dest.FinishDate, act => act.MapFrom(src => src.FinishDate.ToDateTime(TimeOnly.MinValue)))
                .ReverseMap();
            CreateMap<CourseCalendar, CalendarBaseInfo>()
                .ForMember(dest => dest.StartDate, act => act.MapFrom(src => src.StartDate.ToDateTime(TimeOnly.MinValue)))
                .ForMember(dest => dest.FinishDate, act => act.MapFrom(src => src.FinishDate.ToDateTime(TimeOnly.MinValue)))
                .ReverseMap();
            CreateMap<CalendarBaseInfo, CourseCalendar>()
                .ForMember(dest => dest.StartDate, act => act.MapFrom(src => DateOnly.FromDateTime(src.StartDate)))
                .ForMember(dest => dest.FinishDate, act => act.MapFrom(src => DateOnly.FromDateTime(src.FinishDate)))
                .ReverseMap();

            /// Schedule
            CreateMap<CourseSchedule, ScheduleInfo>()
                .ForMember(dest => dest.CalendarId, act => act.MapFrom(src => src.CourseCalendarId))
                .ForMember(dest => dest.Course, act => act.MapFrom(src => src.CourseCalendar.Course))
                .ForMember(dest => dest.Teacher, act => act.MapFrom(src => src.CourseCalendar.Teacher))
                .ReverseMap();
            CreateMap<CourseSchedule, ScheduleBaseInfo>()
                .ForMember(dest => dest.CalendarId, act => act.MapFrom(src => src.CourseCalendarId))
                .ReverseMap();

            /// Review
            CreateMap<CourseReview, ReviewInfo>()
                .ForMember(dest => dest.CalendarId, act => act.MapFrom(src => src.CourseCalendarId))
                .ReverseMap();
            CreateMap<CourseReview, ReviewBaseInfo>()
                .ForMember(dest => dest.CalendarId, act => act.MapFrom(src => src.CourseCalendarId))
                .ReverseMap();

            /// Student
            CreateMap<CourseStudent, StudentInfo>()
                .ForMember(dest => dest.ScheduleId, act => act.MapFrom(src => src.CourseScheduleId))
                .ForMember(dest => dest.Friends, act => act.MapFrom(src => src.CourseStudentFriend))
                .ReverseMap();
            CreateMap<CourseStudent, StudentBaseInfo>()
                .ForMember(dest => dest.ScheduleId, act => act.MapFrom(src => src.CourseScheduleId))
                .ForMember(dest => dest.Friends, act => act.MapFrom(src => src.CourseStudentFriend))
                .ReverseMap();
            CreateMap<CourseStudentFriend, FriendInfo>()
                .ForMember(dest => dest.StudentId, act => act.MapFrom(src => src.CourseStudentId))
                .ReverseMap();
            CreateMap<FriendBaseInfo, CourseStudentFriend>().ReverseMap();

            /// StudentGroup
            CreateMap<CourseStudentGroup, StudentGroupInfo>()
                .ForMember(dest => dest.CalendarId, act => act.MapFrom(src => src.CourseCalendarId))
                .ReverseMap();
            CreateMap<StudentGroupCreate, CourseStudentGroup>()
                .ForMember(dest => dest.CourseCalendarId, act => act.MapFrom(src => src.CalendarId))
                .ReverseMap();

            /// Payment-PayPal
            CreateMap<PayPalInfo, Payment>()
                .ForMember(dest => dest.OrderCode, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.PayCode, act => act.MapFrom(src => src.PurchaseUnits.First().ReferenceId))
                
                .ForMember(dest => dest.AmountCode, act => act.MapFrom(src => src.PurchaseUnits.First().Amount.CurrencyCode))
                .ForMember(dest => dest.AmountValue, act => act.MapFrom(src => src.PurchaseUnits.First().Amount.Value))
               
                .ForMember(dest => dest.PaypalFeeCode, act => act.MapFrom(src => src.PurchaseUnits.First().Payments.Captures.First().Amount.CurrencyCode))
                .ForMember(dest => dest.PaypalFeeValue, act => act.MapFrom(src => src.PurchaseUnits.First().Payments.Captures.First().Amount.Value))
                
                .ForMember(dest => dest.PayerEmailAddress, act => act.MapFrom(src => src.Payer == null ? "" : src.Payer.EmailAddress))
                .ReverseMap();
        }
    }
}
