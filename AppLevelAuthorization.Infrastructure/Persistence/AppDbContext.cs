using AppLevelAuthorization.Application.Interfaces;
using AppLevelAuthorization.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppLevelAuthorization.Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<AppUser>, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("AppLevelAuthorization");
        base.OnModelCreating(builder);
    }

    
    public async Task SaveAsync()
        => await SaveChangesAsync();
}