namespace AppLevelAuthorization.Infrastructure.Settings;

public class JwtSettings
{
    public string Key { get; set; }
    public string ValidIssuer { get; set; }
    public string ValidAudience { get; set; }
    public int TokenExpires { get; set; }
    public int RefreshTokenExpires { get; set; }
}