namespace Backend.WebApi.Util.Security
{
    public class TokenConfig
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessExpiration { get; set; }
        public int RefreshTokenDays { get; set; } = 5;
    }
}
