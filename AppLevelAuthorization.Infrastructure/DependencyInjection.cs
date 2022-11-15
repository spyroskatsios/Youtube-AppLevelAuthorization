using System.Text;
using AppLevelAuthorization.Application.Interfaces;
using AppLevelAuthorization.Infrastructure.Identity;
using AppLevelAuthorization.Infrastructure.Persistence;
using AppLevelAuthorization.Infrastructure.Services;
using AppLevelAuthorization.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AppLevelAuthorization.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureSqlServer(configuration);
        services.ConfigureIdentity();
        services.ConfigureJwt(configuration);
        
        services.AddScoped<IAppDbContext, AppDbContext>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddSingleton<ICurrentUserService, CurrentUserService>();
        services.AddScoped<SeedData>();
        
        return services;
    }
    
    private static void ConfigureSqlServer(this IServiceCollection services, IConfiguration configuration) 
        => services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("SqlDb"));
        });

    private static void ConfigureIdentity(this IServiceCollection services)
    {
        var builder = services.AddIdentityCore<AppUser>(o =>
        {
            o.Password.RequireDigit = true;
            o.Password.RequireLowercase = true;
            o.Password.RequireUppercase = true;
            o.Password.RequireNonAlphanumeric = true;
            o.Password.RequiredLength = 6;
            o.User.RequireUniqueEmail = true;
        });

        builder.AddRoles<IdentityRole>();

        builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);

        builder.AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
    }
    
    private static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.GetSection(nameof(JwtSettings)).Bind(jwtSettings);

        services.AddSingleton(jwtSettings);

        services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwtSettings.ValidIssuer,
                    ValidAudience = jwtSettings.ValidAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),

                    ClockSkew = TimeSpan.Zero
                };
            });
    }

}