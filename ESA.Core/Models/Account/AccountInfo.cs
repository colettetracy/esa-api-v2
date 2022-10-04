namespace ESA.Core.Models.Account
{
    public class AccountInfo
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string ProfilePicture { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public DateTime LastUpdate { get; set; }

        public AccountProfileInfo? Profile { get; set; }

        public RoleInfo? Role { get; set; }
    }

    public class AccountProfileInfo
    {
        public int Id { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string NationalityCode { get; set; } = string.Empty;

        public string CityName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string AboutMe { get; set; } = string.Empty;
        
        public string ZoomLink { get; set; } = string.Empty;

        public DateTime LastUpdate { get; set; }
    }

    public class RoleInfo
    {
        public short Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
