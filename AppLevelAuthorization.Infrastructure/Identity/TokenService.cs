using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AppLevelAuthorization.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace AppLevelAuthorization.Infrastructure.Identity;

public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<AppUser> _userManager;

    public TokenService(JwtSettings jwtSettings, UserManager<AppUser> userManager)
    {
        _jwtSettings = jwtSettings;
        _userManager = userManager;
    }
    
    public async Task<string> GenerateTokenAsync(AppUser user)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaimsForUserAsync(user);
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return token;
    }
    
    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken
        (
            issuer: _jwtSettings.ValidIssuer,
            audience: _jwtSettings.ValidAudience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_jwtSettings.TokenExpires)),
            signingCredentials: signingCredentials
        );

        return tokenOptions;
    }
    
    private async Task<List<Claim>> GetClaimsForUserAsync(AppUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.UserName)
        };

        var otherClaims = await _userManager.GetClaimsAsync(user);
        claims.AddRange(otherClaims);

        var roles = await _userManager.GetRolesAsync(user);

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return claims;
    }

    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);
        var secretKey = new SymmetricSecurityKey(key);

        return new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
    }

}