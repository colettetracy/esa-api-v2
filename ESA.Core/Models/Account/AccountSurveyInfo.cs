using System.ComponentModel.DataAnnotations;

namespace ESA.Core.Models.Account
{
    public class AccountSurveyInfo
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public string MediaInfo { get; set; } = string.Empty;

        public string? RecommendedBy { get; set; }

        public DateTime LastUpdate { get; set; }

        public AccountInfo? Account { get; set; }
    }

    public class AccountSurveyBaseInfo
    {
        [Required]
        public int AccountId { get; set; }

        [Required]
        public string MediaInfo { get; set; } = string.Empty;

        public string? RecommendedBy { get; set; }
    }
}
