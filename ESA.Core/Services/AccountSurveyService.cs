using AutoMapper;

using ESA.Core.Interfaces;
using ESA.Core.Models.Account;
using ESA.Core.Models.Course;
using ESA.Core.Specs;
using ESA.Core.Validators;
using FluentValidation;
using GV.DomainModel.SharedKernel.Extensions;
using GV.DomainModel.SharedKernel.Interfaces;
using GV.DomainModel.SharedKernel.Interop;

namespace ESA.Core.Services
{
    public class AccountSurveyService : IAccountSurveyService
    {
        private readonly IMapper mapper;
        private readonly IAppLogger<AccountSurveyService> logger;
        private readonly IRepository<AccountSurvey> surveyWriteRepository;
        private readonly IReadRepository<AccountSurvey> surveyReadRepository;

        public AccountSurveyService(
            IMapper mapper, 
            IAppLogger<AccountSurveyService> logger, 
            IRepository<AccountSurvey> surveyWriteRepository, 
            IReadRepository<AccountSurvey> surveyReadRepository)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.surveyWriteRepository = surveyWriteRepository;
            this.surveyReadRepository = surveyReadRepository;
        }

        public async Task<Result<AccountSurveyInfo>> AddSurverAsync(AccountSurveyBaseInfo surveyInfo)
        {
            var result = new Result<AccountSurveyInfo>();
            try
            {
                if (surveyInfo == null)
                    return result.Invalid(new List<ValidationError>
                    {
                        new ValidationError()
                        {
                            Identifier = $"Survey::{AddSurverAsync}",
                            ErrorMessage = "Model invalid",
                            Severity = ValidationSeverity.Warning
                        }
                    });

                var validatorInfo = new AccountSurveyValidator();
                var validationInfo = validatorInfo.Validate(surveyInfo);
                if (!validationInfo.IsValid)
                    return result.Invalid(validationInfo.AsErrors());

                var survey = mapper.Map<AccountSurvey>(surveyInfo);
                if (survey == null)
                    return result.Conflict("Mapping error");

                survey.LastUpdate = DateTime.UtcNow;
                survey = await surveyWriteRepository.AddAsync(survey);
                if (survey == null)
                    return result.Conflict("Saving error");

                return result.Success(mapper.Map<AccountSurveyInfo>(survey));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, surveyInfo);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }

        public async Task<Result<AccountSurveyInfo>> FindByAccountAsync(int accountId)
        {
            var result = new Result<AccountSurveyInfo>();
            try
            {
                var survey = await surveyReadRepository.FirstOrDefaultAsync(new AccountSurveySpec(accountId));
                if (survey == null)
                    return result.NotFound("");

                return result.Success(mapper.Map<AccountSurveyInfo>(survey));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, accountId);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }
    }
}
