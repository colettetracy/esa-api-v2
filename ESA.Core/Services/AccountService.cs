using AutoMapper;
using ESA.Core.Entities;
using ESA.Core.Interfaces;
using ESA.Core.Models.Account;
using ESA.Core.Models.Course;
using ESA.Core.Specs;
using ESA.Core.Validators;
using GV.DomainModel.SharedKernel.Extensions;
using GV.DomainModel.SharedKernel.Interfaces;
using GV.DomainModel.SharedKernel.Interop;

namespace ESA.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper mapper;
        private readonly IAppLogger<AccountService> logger;
        private readonly IRepository<Account> accountWriteRepository;
        private readonly IReadRepository<Account> accountReadRepository;
        private readonly INotificationService notificationService;

        public AccountService(IMapper mapper, IAppLogger<AccountService> logger, IRepository<Account> accountWriteRepository, IReadRepository<Account> accountReadRepository, INotificationService notificationService)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.accountWriteRepository = accountWriteRepository;
            this.accountReadRepository = accountReadRepository;
            this.notificationService = notificationService;
        }

        public async Task<Result<AccountInfo>> AddAccountAsync(AccountBaseInfo accountBaseInfo)
        {
            var result = new Result<AccountInfo>();
            try
            {
                if (accountBaseInfo == null)
                    return result.Invalid(new List<ValidationError>
                    {
                        new ValidationError()
                        {
                            Identifier = $"Account::{AddAccountAsync}",
                            ErrorMessage = "Model invalid",
                            Severity = ValidationSeverity.Warning
                        }
                    });

                var validatorInfo = new AccountValidator();
                var validationInfo = validatorInfo.Validate(accountBaseInfo);
                if (!validationInfo.IsValid)
                    return result.Invalid(validationInfo.AsErrors());

                var account = mapper.Map<Account>(accountBaseInfo);
                if (account == null)
                    return result.Conflict("Mapping error");

                account.IsActive = true;
                account.LastUpdate = DateTime.UtcNow;
                if (accountBaseInfo.Picture.StartsWith("http"))
                    account.ProfilePicturePath = accountBaseInfo.Picture;
                else account.ProfilePicture = Convert.FromBase64String(accountBaseInfo.Picture);

                account = await accountWriteRepository.AddAsync(account, Utils.Commons.GetCancellationToken(15).Token);
                if (account == null)
                    return result.Conflict("Save error");

                return result.Success(mapper.Map<AccountInfo>(account));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, accountBaseInfo);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }

        public async Task<Result<ScheduleDeleteInfo>> DeleteAccountAsync(int accountId)
        {
            var result = new Result<ScheduleDeleteInfo>();
            try
            {
                var exists = await accountReadRepository.FirstOrDefaultAsync(new AccountSpec(accountId));
                if (exists == null)
                    return result.NotFound("");
                exists.IsActive = false;
                await accountWriteRepository.UpdateAsync(exists);
                return result.Success(new ScheduleDeleteInfo
                {
                    Deleted = true
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, accountId);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }

        public async Task<Result<AccountInfo>> FindByEmailAsync(string email)
        {
            var result = new Result<AccountInfo>();
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return result.Invalid(new List<ValidationError>
                    {
                        new ValidationError()
                        {
                            Identifier = $"Account::{nameof(FindByEmailAsync)}",
                            ErrorMessage = "Email invalid",
                            Severity = ValidationSeverity.Warning
                        }
                    });

                var account = await accountReadRepository.FirstOrDefaultAsync(new AccountSpec(email), Utils.Commons.GetCancellationToken(15).Token);
                if (account == null)
                    return result.NotFound($"Account not exist: {email}");

                return result.Success(mapper.Map<AccountInfo>(account));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, email);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }

        public async Task<Result<AccountInfo>> FindByIdAsync(int accountId)
        {
            var result = new Result<AccountInfo>();
            try
            {
                if (accountId <= 0)
                    return result.Invalid(new List<ValidationError>
                    {
                        new ValidationError()
                        {
                            Identifier = $"Account::{nameof(FindByIdAsync)}",
                            ErrorMessage = "AccountId invalid",
                            Severity = ValidationSeverity.Warning
                        }
                    });

                var account = await accountReadRepository.FirstOrDefaultAsync(new AccountSpec(accountId), Utils.Commons.GetCancellationToken(15).Token);
                if (account == null)
                    return result.NotFound($"Account not exist: {accountId}");

                return result.Success(mapper.Map<AccountInfo>(account));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, accountId);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }

        public async Task<Result<IEnumerable<AccountInfo>>> FindByRoleAsync(short roleId)
        {
            var result = new Result<IEnumerable<AccountInfo>>();
            try
            {
                if (roleId <= 0)
                    return result.Invalid(new List<ValidationError>
                    {
                        new ValidationError()
                        {
                            Identifier = $"Account::{nameof(FindByRoleAsync)}",
                            ErrorMessage = "RoleId invalid",
                            Severity = ValidationSeverity.Warning
                        }
                    });

                var accounts = await accountReadRepository.ListAsync(new AccountSpec(roleId), Utils.Commons.GetCancellationToken(15).Token);
                if (accounts == null)
                    return result.NotFound($"Account not exist: {roleId}");

                return result.Success(accounts.Select(x => mapper.Map<AccountInfo>(x)));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, roleId);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }

        public async Task<Result<AccountInfo>> UpdateAccountAsync(int accountId, AccountBaseInfo accountBaseInfo)
        {
            var result = new Result<AccountInfo>();
            try
            {
                if (accountId <= 0 || accountBaseInfo == null)
                    return result.Invalid(new List<ValidationError>
                    {
                        new ValidationError()
                        {
                            Identifier = $"Account::{UpdateAccountAsync}",
                            ErrorMessage = "Model invalid",
                            Severity = ValidationSeverity.Warning
                        }
                    });

                var validatorInfo = new AccountValidator();
                var validationInfo = validatorInfo.Validate(accountBaseInfo);
                if (!validationInfo.IsValid)
                    return result.Invalid(validationInfo.AsErrors());

                var account = await accountReadRepository.GetByIdAsync(accountId, Utils.Commons.GetCancellationToken(15).Token);
                if (account == null)
                    return result.NotFound($"Account not exist: {accountId}");

                account.RoleId = accountBaseInfo.RoleId;
                account.FirstName = accountBaseInfo.FirstName;
                account.LastName = accountBaseInfo.LastName;
                account.Email = accountBaseInfo.Email;
                account.Password = accountBaseInfo.Password;
                account.IsActive = accountBaseInfo.IsActive;
                account.LastUpdate = DateTime.UtcNow;

                if (accountBaseInfo.Picture.StartsWith("http"))
                    account.ProfilePicturePath = accountBaseInfo.Picture;
                else account.ProfilePicture = Convert.FromBase64String(accountBaseInfo.Picture);

                if (!account.IsActive)
                    await notificationService.AccountDeleteNotification(accountId);

                await accountWriteRepository.UpdateAsync(account, Utils.Commons.GetCancellationToken(15).Token);
                return result.Success(mapper.Map<AccountInfo>(account));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, accountBaseInfo);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }
    }
}
