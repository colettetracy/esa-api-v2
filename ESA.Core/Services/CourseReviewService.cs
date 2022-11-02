using AutoMapper;

using ESA.Core.Interfaces;
using ESA.Core.Models.Course;
using ESA.Core.Specs;
using ESA.Core.Specs.Filters;
using ESA.Core.Validators;
using GV.DomainModel.SharedKernel.Extensions;
using GV.DomainModel.SharedKernel.Interfaces;
using GV.DomainModel.SharedKernel.Interop;

namespace ESA.Core.Services
{
    public class CourseReviewService : IReviewService
    {
        private readonly IMapper mapper;
        private readonly IAppLogger<CourseReviewService> logger;
        private readonly IRepository<CourseReview> reviewWriteRepository;
        private readonly IReadRepository<CourseReview> reviewReadRepository;

        public CourseReviewService(
            IMapper mapper, 
            IAppLogger<CourseReviewService> logger, 
            IRepository<CourseReview> reviewWriteRepository, 
            IReadRepository<CourseReview> reviewReadRepository)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.reviewWriteRepository = reviewWriteRepository;
            this.reviewReadRepository = reviewReadRepository;
        }

        public async Task<Result<ReviewInfo>> AddReviewAsync(ReviewBaseInfo reviewInfo)
        {
            var result = new Result<ReviewInfo>();
            try
            {
                if (reviewInfo == null)
                    return result.Invalid(new List<ValidationError>
                    {
                        new ValidationError()
                        {
                            Identifier = $"Review::{AddReviewAsync}",
                            ErrorMessage = "Model invalid",
                            Severity = ValidationSeverity.Warning
                        }
                    });

                var validatorInfo = new ReviewValidator();
                var validationInfo = validatorInfo.Validate(reviewInfo);
                if (!validationInfo.IsValid)
                    return result.Invalid(validationInfo.AsErrors());

                var review = mapper.Map<CourseReview>(reviewInfo);
                if (review == null)
                    return result.Conflict("Mapping error");

                review.LastUpdate = DateTime.UtcNow;
                review = await reviewWriteRepository.AddAsync(review);
                if (review == null)
                    return result.Conflict("Saving error");

                return result.Success(mapper.Map<ReviewInfo>(review));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, reviewInfo);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }

        public async Task<Result<IEnumerable<ReviewInfo>>> FilterAsync(ReviewFilter filter)
        {
            var result = new Result<IEnumerable<ReviewInfo>>();
            try
            {
                var list = await reviewReadRepository.ListAsync(new ReviewSpec(filter));
                if (list == null)
                    return result.NotFound("");

                return result.Success(list.Select(x => mapper.Map<ReviewInfo>(x)));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message, filter);
                return result.Error("An unexpected error has occurred", ex.Message);
            }
        }
    }
}
