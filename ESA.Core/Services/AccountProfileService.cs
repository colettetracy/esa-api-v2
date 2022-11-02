using AutoMapper;

using ESA.Core.Interfaces;
using ESA.Core.Models.Account;
using ESA.Core.Specs;
using ESA.Core.Validators;
using GV.DomainModel.SharedKernel.Extensions;
using GV.DomainModel.SharedKernel.Interfaces;
using GV.DomainModel.SharedKernel.Interop;

namespace ESA.Core.Services
{
    public class AccountProfileService : IAccountProfileService
    {
        private readonly IMapper mapper;
        private readonly IAppLogger<AccountProfileService> logger;
        private readonly IRepository<AccountProfile> profileWriteRepository;
        private readonly IReadRepository<AccountProfile> profileReadRepository;

        public AccountProfileService(
            IMapper mapper, 
            IAppLogger<AccountProfileService> logger, 
            IRepository<AccountProfile> profileWriteRepository, 
            IReadRepository<AccountProfile> profileReadRepository)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.profileWriteRepository = profileWriteRepository;
            this.profileReadRepository = profileReadRepository;
        }

        public async Task<Result<AccountProfileInfo>> AddProfileAsync(AccountProfileBase profileInfo)
        {
            var result = new Result<AccountProfileInfo>();
            try
            {
                if (profileInfo == null)
                    return result.Invalid(new List<ValidationError>
                    {
                        new ValidationError()
                        {
                            Identifier = $"Profile::{AddProfileAsync}",
                            ErrorMessage = "Model invalid",
                            Severity = ValidationSeverity.Warning
                        }
                    });

                var validatorInfo = new AccountProfileValidator();
                var validationInfo = validatorInfo.Validate(profileInfo);
                if (!validationInfo.IsValid)
                    return result.Invalid(validationInfo.AsErrors());

                var profile = mapper.Map<AccountProfile>(profileInfo);
                if (profile == null)
                    return result.Conflict("Mapping error");

                profile.LastUpdate = DateTime.UtcNow;
                profile = await profileWriteRepository.AddAsync(profile, Utils.Commons.GetCancellationToken(15).Token);
                if (profile == null)
                    return result.Conflict("Save error");

                return result.Success(mapper.Map<AccountProfileInfo>(profile));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, profileInfo);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }

        public async Task<Result<AccountProfileInfo>> FindByAccountAsync(int accountId)
        {
            var result = new Result<AccountProfileInfo>();
            try
            {
                if (accountId <= 0)
                    return result.Invalid(new List<ValidationError>
                    {
                        new ValidationError()
                        {
                            Identifier = $"Profile::{nameof(FindByAccountAsync)}",
                            ErrorMessage = "AccountId invalid",
                            Severity = ValidationSeverity.Warning
                        }
                    });

                var profile = await profileReadRepository.FirstOrDefaultAsync(new AccountProfileSpec(accountId), Utils.Commons.GetCancellationToken(15).Token);
                if (profile == null)
                    return result.NotFound($"Profile not exist: {accountId}");

                return result.Success(mapper.Map<AccountProfileInfo>(profile));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, accountId);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }

        public async Task<Result<AccountProfileInfo>> UpdateProfileAsync(int profileId, AccountProfileBase profileInfo)
        {
            var result = new Result<AccountProfileInfo>();
            try
            {
                if (profileId <= 0 || profileInfo == null)
                    return result.Invalid(new List<ValidationError>
                    {
                        new ValidationError()
                        {
                            Identifier = $"Profile::{UpdateProfileAsync}",
                            ErrorMessage = "Model invalid",
                            Severity = ValidationSeverity.Warning
                        }
                    });

                var validatorInfo = new AccountProfileValidator();
                var validationInfo = validatorInfo.Validate(profileInfo);
                if (!validationInfo.IsValid)
                    return result.Invalid(validationInfo.AsErrors());

                var profile = await profileReadRepository.GetByIdAsync(profileId, Utils.Commons.GetCancellationToken(15).Token);
                if (profile == null)
                    return result.NotFound($"Profile not exist: {profileId}");

                profile.AboutMe = profileInfo.AboutMe;
                profile.CityName = profileInfo.CityName;
                profile.DateOfBirth = DateOnly.FromDateTime(profileInfo.DateOfBirth);
                profile.NationalityCode = profileInfo.NationalityCode;
                profile.PhoneNumber = profileInfo.PhoneNumber;
                profile.ZoomLink = profileInfo.ZoomLink;
                profile.LastUpdate = DateTime.UtcNow;

                await profileWriteRepository.UpdateAsync(profile, Utils.Commons.GetCancellationToken(15).Token);
                return result.Success(mapper.Map<AccountProfileInfo>(profile));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, profileInfo);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }
    }
}
