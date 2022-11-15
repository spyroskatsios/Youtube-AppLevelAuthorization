namespace AppLevelAuthorization.Application.ServiceResults;

public class AuthResult
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();
}