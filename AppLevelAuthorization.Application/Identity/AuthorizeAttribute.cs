namespace AppLevelAuthorization.Application.Identity;

public class AuthorizeAttribute : Attribute
{
    public EnRole Role { get; set; }

    public AuthorizeAttribute()
    {
    }
}