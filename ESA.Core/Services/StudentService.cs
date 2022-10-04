using AutoMapper;
using ESA.Core.Entities;
using ESA.Core.Interfaces;
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

        public StudentService(
            IMapper mapper, 
            IAppLogger<StudentService> logger, 
            IRepository<CourseStudent> studentWriteRepository, 
            IReadRepository<CourseStudent> studentReadRepository, 
            IRepository<CourseStudentFriend> friendWriteRepository, 
            IReadRepository<CourseStudentFriend> friendReadRepository)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.studentWriteRepository = studentWriteRepository;
            this.studentReadRepository = studentReadRepository;
            this.friendWriteRepository = friendWriteRepository;
            this.friendReadRepository = friendReadRepository;
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
