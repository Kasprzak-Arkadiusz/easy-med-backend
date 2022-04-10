namespace EasyMed.Application.Utils.SecurityTokens;

public class AccessTokenSettings
{
    public string Key { get; set; }
    public int ExpiryTimeInMinutes { get; set; }
    public string JwtIssuer { get; set; }
}