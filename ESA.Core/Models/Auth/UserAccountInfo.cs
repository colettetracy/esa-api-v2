using ESA.Core.Models.Session;

namespace ESA.Core.Models.Auth
{
    public class UserAccountInfo
    {
        public int Id { get; set; } = 0;

        public int RoleId { get; set; } = 0;

        public string Username { get; set; } = string.Empty;

        public JwtResponse? Authorization { get; set; }
    }
}
