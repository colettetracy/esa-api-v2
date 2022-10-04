using ESA.Core.Models.Account;
using GV.DomainModel.SharedKernel.Interop;

namespace ESA.Core.Interfaces
{
    public interface IAccountService
    {
        Task<Result<IEnumerable<AccountInfo>>> FindByRoleAsync(short roleId);

        Task<Result<AccountInfo>> FindByIdAsync(int accountId);

        Task<Result<AccountInfo>> FindByEmailAsync(string email);

        Task<Result<AccountInfo>> AddAccountAsync(AccountBaseInfo accountBaseInfo);

        Task<Result<AccountInfo>> UpdateAccountAsync(int accountId, AccountBaseInfo accountBaseInfo);
    }

    public interface IAccountSurveyService
    {
        Task<Result<AccountSurveyInfo>> FindByAccountAsync(int accountId);

        Task<Result<AccountSurveyInfo>> AddSurverAsync(AccountSurveyBaseInfo surveyInfo);
    }
}
