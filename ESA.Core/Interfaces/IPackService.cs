using ESA.Core.Models.Course;
using ESA.Core.Models.Payment;
using GV.DomainModel.SharedKernel.Interop;

namespace ESA.Core.Interfaces
{
    public interface IPackService
    {
        Task<Result<IEnumerable<PackInfo>>> GetAllAsync();
        Task<Result<PackInfo>> FindByIdAsync(int packId);
        Task<Result<PackInfo>> AddPackAsync(PackBaseInfo packBaseInfo);
        Task<Result<PackInfo>> UpdatePackAsync(PackBaseInfo packBaseInfo, int id);

        Task<Result<ScheduleDeleteInfo>> DeletePackAsync(int packId);
    }
}
