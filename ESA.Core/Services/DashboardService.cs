using ESA.Core.Interfaces;
using ESA.Core.Models.Payment;
using ESA.Core.Models.Student;
using ESA.Core.Specs;
using ESA.Core.Specs.Filters;
using GV.DomainModel.SharedKernel.Interfaces;
using GV.DomainModel.SharedKernel.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESA.Core.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IReadRepository<CourseStudent> studentReadRepository;
        private readonly IReadRepository<StudentPacks> packReadRepository;
        private readonly IReadRepository<Account> accountReadRepository;
        private readonly IReadRepository<CourseReview> reviewReadRepository;

        public DashboardService(IReadRepository<CourseStudent> studentReadRepository, IReadRepository<StudentPacks> packReadRepository, IReadRepository<Account> accountReadRepository, IReadRepository<CourseReview> reviewReadRepository)
        {
            this.studentReadRepository = studentReadRepository;
            this.packReadRepository = packReadRepository;
            this.accountReadRepository = accountReadRepository;
            this.reviewReadRepository = reviewReadRepository;
        }

        public async Task<Result<DashboardInfo>> GetInfo()
        {
            var result = new Result<DashboardInfo>();

            var info = new DashboardInfo(); 

            var filter1 = new StudentFilter() { PaymentConfirmed = true };
            var courses = await studentReadRepository.ListAsync(new StudentSpec(filter1));
            info.Courses = courses.Count();

            //var filter2 = new AccountFilter() { PaymentConfirmed = true };
            short roleId = 3;
            var students = await accountReadRepository.ListAsync(new AccountSpec(roleId), Utils.Commons.GetCancellationToken(15).Token);
            info.Students = students.Count();

            var reviews = await reviewReadRepository.ListAsync();
            info.Reviews = reviews.Count();

            var packs = await packReadRepository.ListAsync();
            info.Packs = packs.Count();

            return result.Success(info);
        }
    }
}
