namespace ESA.Core.Models.Session
{
    public class JwtSetting
    {
        public string Key { get; set; } = string.Empty;

        public string Issuer { get; set; } = string.Empty;

        public string Audience { get; set; } = string.Empty;

        public int DurationMinutes { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string ServiceName { get; set; } = string.Empty;
    }
}
