namespace AppLevelAuthorization.Api.Responses;

public class AuthResponse
{
    public bool Success { get; set; }
    public string? Token { get; set; }
    public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();
}