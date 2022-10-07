using ESA.Core.Models.Account;
using ESA.Core.Models.Course;
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

        Task<Result<ScheduleDeleteInfo>> DeleteAccountAsync(int accountId);
    }

    public interface IAccountSurveyService
    {
        Task<Result<AccountSurveyInfo>> FindByAccountAsync(int accountId);

        Task<Result<AccountSurveyInfo>> AddSurverAsync(AccountSurveyBaseInfo surveyInfo);
    }
}
