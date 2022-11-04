using AutoMapper;
using ESA.Core.Interfaces;
using ESA.Core.Models.Course;
using ESA.Core.Models.Student;
using ESA.Core.Specs;
using ESA.Core.Specs.Filters;
using ESA.Core.Validators;
using GV.DomainModel.SharedKernel.Extensions;
using GV.DomainModel.SharedKernel.Interfaces;
using GV.DomainModel.SharedKernel.Interop;

namespace ESA.Core.Services
{
    public class StudentService : IStudentService
    {
        private readonly IMapper mapper;
        private readonly IAppLogger<StudentService> logger;
        private readonly IRepository<CourseStudent> studentWriteRepository;
        private readonly IReadRepository<CourseStudent> studentReadRepository;
        private readonly IRepository<CourseStudentFriend> friendWriteRepository;
        private readonly IReadRepository<CourseStudentFriend> friendReadRepository;
        private readonly IReadRepository<Coupons> couponReadRepository;
        public StudentService(
            IMapper mapper,
            IAppLogger<StudentService> logger,
            IRepository<CourseStudent> studentWriteRepository,
            IReadRepository<CourseStudent> studentReadRepository,
            IRepository<CourseStudentFriend> friendWriteRepository,
            IReadRepository<CourseStudentFriend> friendReadRepository,
         IReadRepository<Coupons> couponReadRepository)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.studentWriteRepository = studentWriteRepository;
            this.studentReadRepository = studentReadRepository;
            this.friendWriteRepository = friendWriteRepository;
            this.friendReadRepository = friendReadRepository;
            this.couponReadRepository = couponReadRepository;
        }

        public async Task<Result<StudentInfo>> AddStudentAsync(StudentBaseInfo studentInfo)
        {
            var result = new Result<StudentInfo>();
            try
            {
                if (studentInfo == null)
                    return result.Invalid(new List<ValidationError>
                    {
                        new ValidationError()
                        {
                            Identifier = $"Student::{AddStudentAsync}",
                            ErrorMessage = "Model invalid",
                            Severity = ValidationSeverity.Warning
                        }
                    });

                var validatorInfo = new StudentValidator();
                var validationInfo = validatorInfo.Validate(studentInfo);
                if (!validationInfo.IsValid)
                    return result.Invalid(validationInfo.AsErrors());



                var student = mapper.Map<CourseStudent>(studentInfo);
                if (student == null)
                    return result.Conflict("Mapping error");

                Coupons? coupon = new();
                if (studentInfo.Coupon != null)
                    coupon = await couponReadRepository.FirstOrDefaultAsync(new CouponSpec(studentInfo.Coupon), Utils.Commons.GetCancellationToken(15).Token);
                if (coupon == null)
                    return result.NotFound($"Coupon not exist: {studentInfo.Coupon}");
                else
                    student.Discount = (coupon.Discount / 100) * student.Amount;

                student.LastUpdate = DateTime.UtcNow;
                student = await studentWriteRepository.AddAsync(student);
                if (student == null)
                    return result.Conflict("Saving error");

                return result.Success(mapper.Map<StudentInfo>(student));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, studentInfo);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }

        public async Task<Result<StudentInfo>> ApplyCouponAsync(StudentCouponBaseInfo studentInfo)
        {
            var result = new Result<StudentInfo>();
            try
            {
                if (studentInfo == null)
                    return result.Invalid(new List<ValidationError>
                    {
                        new ValidationError()
                        {
                            Identifier = $"Student::{AddStudentAsync}",
                            ErrorMessage = "Model invalid",
                            Severity = ValidationSeverity.Warning
                        }
                    });

                var validatorInfo = new StudentCouponValidator();
                var validationInfo = validatorInfo.Validate(studentInfo);
                if (!validationInfo.IsValid)
                    return result.Invalid(validationInfo.AsErrors());

                var student = await studentReadRepository.FirstOrDefaultAsync(new StudentSpec(studentInfo.Id), Utils.Commons.GetCancellationToken(15).Token);
                if (student == null)
                    return result.Conflict("Student schedule not found");
                if(student.Discount >0)
                {
                    return result.Conflict("Coupon has been added yet.");
                }
                var coupon = await couponReadRepository.FirstOrDefaultAsync(new CouponSpec(studentInfo.Coupon), Utils.Commons.GetCancellationToken(15).Token);
                if (coupon == null)
                    return result.NotFound($"Coupon not exist: {studentInfo.Coupon}");

                if(!coupon.IsActive)
                {
                    return result.Conflict("The coupon is not available!.");
                }

                decimal desc = decimal.Divide(coupon.Discount , 100);
                student.Discount = (desc * student.Amount);

                student.LastUpdate = DateTime.UtcNow;
                await studentWriteRepository.UpdateAsync(student);

                return result.Success(mapper.Map<StudentInfo>(student));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, studentInfo);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }

        public async Task<Result<ScheduleDeleteInfo>> DeleteStudentAsync(int id)
        {
            var result = new Result<ScheduleDeleteInfo>();
            try
            {
                var exists = await studentReadRepository.FirstOrDefaultAsync(new StudentSpec(id));
                if (exists == null)
                    return result.NotFound("");
                await studentWriteRepository.DeleteAsync(exists);
                return result.Success(new ScheduleDeleteInfo
                {
                    Deleted = true
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, id);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }

        public async Task<Result<IEnumerable<StudentInfo>>> FilterAsync(StudentFilter filter)
        {
            var result = new Result<IEnumerable<StudentInfo>>();
            try
            {
                var list = await studentReadRepository.ListAsync(new StudentSpec(filter));
                if (list == null)
                    return result.NotFound("");

                return result.Success(list.Select(x => mapper.Map<StudentInfo>(x)));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, filter);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }
    }
}
