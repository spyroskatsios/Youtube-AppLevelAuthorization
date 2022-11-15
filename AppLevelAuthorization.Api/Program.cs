using AppLevelAuthorization.Api.Middlewares;
using AppLevelAuthorization.Application;
using AppLevelAuthorization.Infrastructure;
using AppLevelAuthorization.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseMiddleware<CustomExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var seed = app.Services.CreateScope().ServiceProvider.GetRequiredService<SeedData>();

await seed.SetUpRoles();
await seed.CreateAdminsAsync();
await seed.CreateManagersAsync();

app.Run();