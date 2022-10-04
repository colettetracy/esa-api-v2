using System.ComponentModel.DataAnnotations;

namespace ESA.Core.Models.Account
{
    public class AccountBaseInfo
    {
        [Required]
        public short RoleId { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        public string? Password { get; set; }

        [Required]
        public string Picture { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }

    public class AccountProfileBase
    {
        [Required]
        public int AccountId { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string NationalityCode { get; set; } = string.Empty;

        public string? CityName { get; set; }

        public string? PhoneNumber { get; set; }

        [Required]
        public string AboutMe { get; set; } = string.Empty;

        public string? ZoomLink { get; set; }
    }
}
