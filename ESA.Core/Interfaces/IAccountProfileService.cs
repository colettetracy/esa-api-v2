using ESA.Core.Models.Account;
using GV.DomainModel.SharedKernel.Interop;

namespace ESA.Core.Interfaces
{
    public interface IAccountProfileService
    {
        Task<Result<AccountProfileInfo>> FindByAccountAsync(int accountId);

        Task<Result<AccountProfileInfo>> AddProfileAsync(AccountProfileBase profileInfo);

        Task<Result<AccountProfileInfo>> UpdateProfileAsync(int profileId, AccountProfileBase profileInfo);
    }
}
