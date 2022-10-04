namespace ESA.Core.Models.Session
{
    public class JwtResponse
    {
        public string AccessToken { get; set; } = string.Empty;

        public string TokenType { get; set; } = string.Empty;

        public DateTime Expires { get; set; }
    }
}
