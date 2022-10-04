using Ardalis.Specification.EntityFrameworkCore;
using GV.DomainModel.SharedKernel.Interfaces;

namespace ESA.Infrastructure.Data
{
    public class AcademyRepository<T> : RepositoryBase<T>, IRepository<T>, IReadRepository<T> where T : class
    {
        public AcademyRepository(AcademyDbContext dbContext) : base(dbContext)
        {
        }
    }
}
