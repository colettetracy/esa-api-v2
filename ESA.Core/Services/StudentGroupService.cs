﻿using AutoMapper;

using ESA.Core.Interfaces;
using ESA.Core.Models.Course;
using ESA.Core.Models.Student;
using ESA.Core.Specs;
using ESA.Core.Specs.Filters;
using ESA.Core.Validators;
using FluentValidation;
using GV.DomainModel.SharedKernel.Extensions;
using GV.DomainModel.SharedKernel.Interfaces;
using GV.DomainModel.SharedKernel.Interop;

namespace ESA.Core.Services
{
    public class StudentGroupService : IStudentGroupService
    {
        private readonly IMapper mapper;
        private readonly IAppLogger<StudentGroupService> logger;
        private readonly IRepository<CourseStudentGroup> groupWriteRepository;
        private readonly IReadRepository<CourseStudentGroup> groupReadRepository;

        public StudentGroupService(
            IMapper mapper, 
            IAppLogger<StudentGroupService> logger, 
            IRepository<CourseStudentGroup> groupWriteRepository, 
            IReadRepository<CourseStudentGroup> groupReadRepository)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.groupWriteRepository = groupWriteRepository;
            this.groupReadRepository = groupReadRepository;
        }

        public async Task<Result<StudentGroupInfo>> AddStudentAsync(StudentGroupCreate studentGroup)
        {
            var result = new Result<StudentGroupInfo>();
            try
            {
                if (studentGroup == null)
                    return result.Invalid(new List<ValidationError>
                    {
                        new ValidationError()
                        {
                            Identifier = $"StudentGroup::{AddStudentAsync}",
                            ErrorMessage = "Model invalid",
                            Severity = ValidationSeverity.Warning
                        }
                    });

                var validatorInfo = new StudentGroupValidator();
                var validationInfo = validatorInfo.Validate(studentGroup);
                if (!validationInfo.IsValid)
                    return result.Invalid(validationInfo.AsErrors());

                var group = mapper.Map<CourseStudentGroup>(studentGroup);
                if (group == null)
                    return result.Conflict("Mapping error");

                group.LastUpdate = DateTime.UtcNow;
                group = await groupWriteRepository.AddAsync(group);
                if (group == null)
                    return result.Conflict("Saving error");

                return result.Success(mapper.Map<StudentGroupInfo>(group));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, studentGroup);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }

        public async Task<Result<IEnumerable<StudentGroupInfo>>> FilterAsync(StudentGroupFilter filter)
        {
            var result = new Result<IEnumerable<StudentGroupInfo>>();
            try
            {
                var list = await groupReadRepository.ListAsync(new StudentGroupSpec(filter));
                if (list == null)
                    return result.NotFound("");

                return result.Success(list.Select(x => mapper.Map<StudentGroupInfo>(x)));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, filter);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }

        public async Task<Result<List<StudentGroupInfo>>> UpdatePaymentAsync(List<PaymentConfirmBaseInfo> studentInfo)
        {
            var result = new Result<List<StudentGroupInfo>>();
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
                List<StudentGroupInfo> students = new();
                foreach (var item in studentInfo)
                {
                    var student = await groupReadRepository.FirstOrDefaultAsync(new StudentGroupSpec(item.Id), Utils.Commons.GetCancellationToken(15).Token);
                    if (student == null)
                        return result.Conflict("Student schedule not found");

                    student.PaymentConfirmed = true;
                    student.LastUpdate = DateTime.UtcNow;

                    await groupWriteRepository.UpdateAsync(student);

                    students.Add(mapper.Map<StudentGroupInfo>(student));
                }
                return result.Success(students);
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
                var exists = await groupReadRepository.FirstOrDefaultAsync(new StudentGroupSpec(id));
                if (exists == null)
                    return result.NotFound("");
                await groupWriteRepository.DeleteAsync(exists);
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
    }
}
