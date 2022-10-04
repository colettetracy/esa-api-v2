namespace ESA.Core.Models.Session
{
    public class JwtInfo
    {
        public int Identifier { get; set; }

        public string ServiceName { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;
    }
}
