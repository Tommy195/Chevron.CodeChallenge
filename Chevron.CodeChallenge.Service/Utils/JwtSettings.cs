namespace Chevron.CodeChallege.Api.Helpers
{
    public static class JwtSettings
    {
        public static string Key { get; set; }
        public static string Issuer { get; set; }
        public static string Audience { get; set; }
        public static int MinutesToExpiration { get; set; }
    }
}
