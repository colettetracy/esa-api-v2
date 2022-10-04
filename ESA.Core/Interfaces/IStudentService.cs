using ESA.Core.Models.Student;
using ESA.Core.Specs.Filters;
using GV.DomainModel.SharedKernel.Interop;

namespace ESA.Core.Interfaces
{
    public interface IStudentService
    {
        Task<Result<IEnumerable<StudentInfo>>> FilterAsync(StudentFilter filter);

        Task<Result<StudentInfo>> AddStudentAsync(StudentBaseInfo studentInfo);
    }

    public interface IStudentGroupService
    {
        Task<Result<IEnumerable<StudentGroupInfo>>> FilterAsync(StudentGroupFilter filter);

        Task<Result<StudentGroupInfo>> AddStudentAsync(StudentGroupCreate studentGroup);
    }
}
